using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MiniUrl.KeyManager.Domain.Models;
using MiniUrl.KeyManager.Infrastructure.EntityConfigurations;

namespace MiniUrl.KeyManager.Infrastructure
{
    public class KeysManagerContext : DbContext
    {
        public DbSet<KeysGeneratorConfiguration> KeyGeneratorConfigurations { get; set; }
        public DbSet<Key> Keys { get; set; }

        public KeysManagerContext(DbContextOptions<KeysManagerContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new KeyEntityTypeConfiguration());
            builder.ApplyConfiguration(new ConfigurationEntityTypeConfiguration());
        }
    }

    public class KeyManagerDesignFactory : IDesignTimeDbContextFactory<KeysManagerContext>
    {
        public KeysManagerContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<KeysManagerContext>()
                .UseNpgsql("Host=localhost;Database=KeysManager;Username=postgres;Password=postgres");

            return new(optionsBuilder.Options);
        }
    }
}
