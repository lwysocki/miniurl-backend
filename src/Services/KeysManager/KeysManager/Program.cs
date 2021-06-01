using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiniUrl.KeyManager.Extensions;
using MiniUrl.KeyManager.Infrastructure;
using MiniUrl.KeyManager.Services;
using System.IO;
using System.Net;

namespace MiniUrl.KeyManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();
            var host = CreateWebHostBuilder(args, configuration).Build();

            host.MigrateDbContext<KeysManagerContext>((context, services) =>
            {
                var logger = services.GetService<ILogger<KeyManagerContextSeed>>();
                var keysGenerator = services.GetService<IKeysGeneratorService>();

                new KeyManagerContextSeed()
                    .SeedAsync(context, keysGenerator, logger)
                    .Wait();
            });

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args, IConfiguration configuration) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureKestrel(options =>
                {
                    var grpcPort = GetGrpcPort(configuration);

                    options.Listen(IPAddress.Any, grpcPort, listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http2;
                    });

                })
                .UseStartup<Startup>();

        private static int GetGrpcPort(IConfiguration configuration)
        {
            return configuration.GetValue("GrpcPort", 5012);
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
