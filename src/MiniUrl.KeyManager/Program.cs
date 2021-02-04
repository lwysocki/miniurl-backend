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
                //TODO: make KeysGenerator a service?
                //KeysGenerator keysGenerator = new KeysGenerator(67645734911, 8192);
                KeysGenerator keysGenerator = new KeysGenerator(10000, 100);
                var keys = keysGenerator.Generate();
                var keyEntitties = keys.Select(k => new Key(k));

                foreach (var e in keyEntitties)
                {
                    context.Keys.Add(e);
                }

                context.SaveChanges();
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
