using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MiniUrl.Url.Domain.Model;
using MiniUrl.Url.Infrastructure.EntityConfigurations;

namespace MiniUrl.Url.Infrastructure
{
    public class UrlContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }

        public UrlContext(DbContextOptions<UrlContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AddressEntityTypeConfiguration());
        }
    }

    public class KeyManagerDesignFactory : IDesignTimeDbContextFactory<UrlContext>
    {
        public UrlContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UrlContext>()
                .UseNpgsql("Host=localhost;Database=Associations;Username=postgres;Password=postgres");

            return new(optionsBuilder.Options);
        }
    }
}
