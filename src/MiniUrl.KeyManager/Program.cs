using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MiniUrl.KeyManager.Domain;
using MiniUrl.KeyManager.Domain.Models;
using MiniUrl.KeyManager.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniUrl.KeyManager.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            using (var context = scope.ServiceProvider.GetService<KeyManagerDbContext>())
            {
                context.Database.EnsureCreated();

                //Restore generator config
                //Create class responsible for initialization, and rely on keys generator being injected?
                var keysGenerator = host.Services.GetService<IKeysGenerator>();
                var keys = keysGenerator.Generate();
                var keyEntitties = keys.Select(k => new Key(k));

                foreach (var e in keyEntitties)
                {
                    context.Keys.Add(e);
                }

                context.KeyManagerConfigurations.Add(new KeyManagerConfiguration() { Value = keysGenerator.ConfigurationJson });

                context.SaveChanges();

                //Save generator config
                //Unit of work pattern?
            }

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
