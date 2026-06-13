using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace MiniUrl.IntegrationTests
{
    public class IntegrationFixture : IAsyncLifetime
    {
        public WebApplicationFactory<KeyManager.Startup> KeyManagerFactory { get; private set; }
        public WebApplicationFactory<ApiGateway.Web.Startup> ApiGatewayFactory { get; private set; }

        public HttpClient KeyManagerClient { get; private set; }
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
                        { "KeysGenerator:Limit", "50000" }, // Down from 67 billion
                        { "KeysGenerator:Step", "1000" }
                    });
                });
            });

            KeyManagerClient = KeyManagerFactory.CreateClient();

            ApiGatewayFactory = new WebApplicationFactory<ApiGateway.Web.Startup>()
            .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureAppConfiguration((context, config) =>
                    {
                        config.AddJsonFile("appsettings.json", false);
                        config.AddEnvironmentVariables();
                    });
                });

            ApiGatewayClient = ApiGatewayFactory.CreateClient();
        }

        public async Task DisposeAsync()
        {
            ApiGatewayClient?.Dispose();
            KeyManagerClient?.Dispose();

            await ApiGatewayFactory.DisposeAsync();
            await KeyManagerFactory.DisposeAsync();
        }
    }
}
