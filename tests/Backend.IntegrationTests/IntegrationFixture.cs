extern alias AssociationProj;
extern alias UrlProj;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;
using Xunit;

namespace MiniUrl.IntegrationTests
{
    public class IntegrationFixture : IAsyncLifetime
    {
        private readonly IContainer _etcdContainer = new ContainerBuilder("quay.io/coreos/etcd:v3.5.0")
            .WithPortBinding(2379, true)
            .WithEnvironment("ALLOW_NONE_AUTHENTICATION", "yes")
            .WithEnvironment("ETCD_LISTEN_CLIENT_URLS", "http://0.0.0.0:2379")
            .WithEnvironment("ETCD_ADVERTISE_CLIENT_URLS", "http://127.0.0.1:2379")
            .Build();
        private readonly PostgreSqlContainer _associationDb = new PostgreSqlBuilder("postgres:18-alpine")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithDatabase("Associations")
            .Build();

        public WebApplicationFactory<AssociationProj::Program> AssociationFactory { get; private set; }
        public WebApplicationFactory<UrlProj::Program> UrlFactory { get; private set; }
        public WebApplicationFactory<ApiGateway.Web.Startup> ApiGatewayFactory { get; private set; }

        public HttpClient AssociationClient { get; private set; }
        public HttpClient UrlClient { get; private set; }
        public HttpClient ApiGatewayClient { get; private set; }

        public const string url = "google.com";

        public async Task InitializeAsync()
        {
            await Task.WhenAll(_associationDb.StartAsync(), _etcdContainer.StartAsync());

            var etcdConnectionString = $"http://{_etcdContainer.Hostname}:{_etcdContainer.GetMappedPublicPort(2379)}";
            var associationConnectionString = _associationDb.GetConnectionString();

            AssociationFactory = new MiniUrlWebApplicationFactory<AssociationProj::Program>(builder =>
            {
                builder.UseEnvironment("Development");
                builder.ConfigureHostConfiguration(config =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { "EtcdConnectionString", etcdConnectionString},
                        { "ConnectionString", associationConnectionString },
                        { "GrpcPort", "5010" }
                    });
                });
            });

            AssociationClient = AssociationFactory.CreateClient();

            UrlFactory = new MiniUrlWebApplicationFactory<UrlProj::Program>(builder =>
            {
                builder.UseEnvironment("Development");
                builder.ConfigureHostConfiguration(config =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { "ConnectionString", associationConnectionString },
                        { "GrpcPort", "5011" }
                    });
                });
            });

            UrlClient = UrlFactory.CreateClient();

            ApiGatewayFactory = new WebApplicationFactory<ApiGateway.Web.Startup>()
            .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((context, config) =>
                    {
                        config.AddJsonFile("appsettings.json", false);
                        config.AddEnvironmentVariables();
                    });

                    builder.ConfigureServices(services =>
                    {
                        var associationHandler = AssociationFactory.Server.CreateHandler();

                        var associationChannel = GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
                        {
                            HttpHandler = associationHandler
                        });

                        services.AddSingleton(sp => new GrpcAssociation.Association.AssociationClient(associationChannel));

                        var urlHandler = UrlFactory.Server.CreateHandler();

                        var urlChannel = GrpcChannel.ForAddress("http://localhost", new GrpcChannelOptions
                        {
                            HttpHandler = urlHandler
                        });

                        services.AddSingleton(sp => new GrpcUrl.Url.UrlClient(urlChannel));
                    });
                });

            ApiGatewayClient = ApiGatewayFactory.CreateClient();
        }

        public async Task DisposeAsync()
        {
            ApiGatewayClient?.Dispose();
            UrlClient?.Dispose();
            AssociationClient?.Dispose();

            await ApiGatewayFactory.DisposeAsync();
            await UrlFactory.DisposeAsync();
            await AssociationFactory.DisposeAsync();

            await Task.WhenAll(_etcdContainer.StopAsync(), _associationDb.StopAsync());

            await _associationDb.DisposeAsync().AsTask();
            await _etcdContainer.DisposeAsync().AsTask();
        }
    }
}
