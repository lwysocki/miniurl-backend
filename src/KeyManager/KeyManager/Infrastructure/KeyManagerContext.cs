using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MiniUrl.KeyManager.Domain.Models;
using MiniUrl.KeyManager.Infrastructure.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniUrl.KeyManager.Infrastructure
{
    public class KeyManagerContext : DbContext
    {
        public DbSet<KeyManagerConfiguration> KeyManagerConfigurations { get; set; }
        public DbSet<Key> Keys { get; set; }

        public KeyManagerContext(DbContextOptions<KeyManagerContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new KeyEntityTypeConfiguration());
            builder.ApplyConfiguration(new KeyManagerConfigurationEntityTypeConfiguration());
        }
    }

    public class KeyManagerDesignFactory : IDesignTimeDbContextFactory<KeyManagerContext>
    {
        public KeyManagerContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<KeyManagerContext>()
                .UseNpgsql("Host=localhost;Database=KeyManager;Username=postgres;Password=postgres");

            return new(optionsBuilder.Options);
        }
    }
}
