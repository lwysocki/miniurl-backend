using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MiniUrl.Association.Domain.Model;
using MiniUrl.Association.Infrastructure.EntityConfigurations;

namespace MiniUrl.Association.Infrastructure
{
    public class KeyContext : DbContext
    {
        public DbSet<Key> Keys { get; set; }

        public KeyContext(DbContextOptions<KeyContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new KeyEntityTypeConfiguration());
        }
    }

    public class KeyDesignFactory : IDesignTimeDbContextFactory<KeyContext>
    {
        public KeyContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<KeyContext>()
                .UseNpgsql("Host=localhost;Database=KeyManager;Username=postgres;Password=postgres");

            return new(optionsBuilder.Options);
        }
    }
}
