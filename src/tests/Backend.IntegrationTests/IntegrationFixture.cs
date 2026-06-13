extern alias AssociationProj;
extern alias UrlProj;

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MiniUrl.IntegrationTests
{
    public class IntegrationFixture : IAsyncLifetime
    {
        public WebApplicationFactory<KeyManager.Startup> KeyManagerFactory { get; private set; }
        public WebApplicationFactory<AssociationProj::MiniUrl.Association.Startup> AssociationFactory { get; private set; }
        public WebApplicationFactory<UrlProj::MiniUrl.Url.Startup> UrlFactory { get; private set; }
        public WebApplicationFactory<ApiGateway.Web.Startup> ApiGatewayFactory { get; private set; }

        public HttpClient KeyManagerClient { get; private set; }
        public HttpClient AssociationClient { get; private set; }
        public HttpClient UrlClient { get; private set; }
        public HttpClient ApiGatewayClient { get; private set; }

        public const string url = "google.com";

        public async Task InitializeAsync()
        {
            KeyManagerFactory = new WebApplicationFactory<KeyManager.Startup>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Development");
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { "ConnectionString", "Host=localhost;Port=5432;Database=KeysManager;Username=postgres;Password=postgres" },
                        { "GrpcPort", "81" },
                        { "KeysGenerator:Iteration", "0" },
                        { "KeysGenerator:Limit", "50000" },
                        { "KeysGenerator:Step", "1000" }
                    });
                });
            });

            KeyManagerClient = KeyManagerFactory.CreateClient();

            AssociationFactory = new WebApplicationFactory<AssociationProj::MiniUrl.Association.Startup>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Development");
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { "AssociationsConnectionString", "Host=localhost;Port=5432;Database=Associations;Username=postgres;Password=postgres" },
                        { "KeysManagerConnectionString", "Host=localhost;Port=5432;Database=KeysManager;Username=postgres;Password=postgres" },
                        { "GrpcPort", "5010" }
                    });
                });
            });

            AssociationClient = AssociationFactory.CreateClient();

            UrlFactory = new WebApplicationFactory<UrlProj::MiniUrl.Url.Startup>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Development");
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { "ConnectionString", "Host=localhost;Port=5432;Database=Associations;Username=postgres;Password=postgres" },
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
            KeyManagerClient?.Dispose();

            await ApiGatewayFactory.DisposeAsync();
            await UrlFactory.DisposeAsync();
            await AssociationFactory.DisposeAsync();
            await KeyManagerFactory.DisposeAsync();
        }
    }
}
