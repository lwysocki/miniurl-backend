using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using MiniUrl.ApiGateway.Web;
using System.IO;
using System.Reflection;

namespace MiniUrl.IntegrationTests.Services.ApiGateway
{
    class ApiGatewaySetup
    {
        public static TestServer CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(ApiGatewaySetup)).Location;

            var webHostBuilder = new WebHostBuilder()
                .UseContentRoot(Path.GetDirectoryName(path))
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddJsonFile("Services/ApiGateway/appsettings.json", false)
                    .AddEnvironmentVariables();
                })
                .UseStartup<Startup>();

            var testServer = new TestServer(webHostBuilder);

            return testServer;
        }
    }
}
