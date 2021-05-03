using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MiniUrl.KeyManager.Domain.Models;
using MiniUrl.KeyManager.Infrastructure.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MiniUrl.KeyManager.Infrastructure
{
    public class KeyManagerContext : DbContext
    {
        public DbSet<KeyGeneratorConfiguration> KeyGeneratorConfigurations { get; set; }
        public DbSet<KeyServiceConfiguration> KeyServiceConfigurations { get; set; }
        public DbSet<Key> Keys { get; set; }

        public KeyManagerContext(DbContextOptions<KeyManagerContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new KeyEntityTypeConfiguration());
            builder.ApplyConfiguration(new ConfigurationEntityTypeConfiguration());
        }
    }

    public class KeyManagerDesignFactory : IDesignTimeDbContextFactory<KeyManagerContext>
    {
        public KeyManagerContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<KeyManagerContext>()
                .UseNpgsql(config["ConnectionString"]);

            return new(optionsBuilder.Options);
        }
    }
}
