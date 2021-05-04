using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiniUrl.KeyManager.Extensions;
using MiniUrl.KeyManager.Infrastructure;
using MiniUrl.KeyManager.Services;

namespace MiniUrl.KeyManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            host.MigrateDbContext<KeyManagerContext>((context, services) =>
            {
                var logger = services.GetService<ILogger<KeyManagerContextSeed>>();
                var keysGenerator = services.GetService<IKeysGeneratorService>();

                new KeyManagerContextSeed()
                    .SeedAsync(context, keysGenerator, logger)
                    .Wait();
            });

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
