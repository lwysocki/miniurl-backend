using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace MiniUrl.IntegrationTests
{
    public class IntegrationFixture : IAsyncLifetime
    {
        public WebApplicationFactory<ApiGateway.Web.Startup> ApiGatewayFactory { get; private set; }

        public HttpClient ApiGatewayClient { get; private set; }

        public const string url = "google.com";

        public async Task InitializeAsync()
        {
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

            await ApiGatewayFactory.DisposeAsync();
        }
    }
}
