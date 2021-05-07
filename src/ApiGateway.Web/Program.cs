using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MiniUrl.ApiGateway.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    var sources = builder.Sources;
                    sources.Insert(3, new Microsoft.Extensions.Configuration.Json.JsonConfigurationSource()
                    {
                        Optional = true,
                        Path = "appsettings.localhost.json",
                        ReloadOnChange = false
                    });
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
