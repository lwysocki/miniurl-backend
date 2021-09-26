using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MiniUrl.Association.Domain.Model;
using MiniUrl.Association.Infrastructure.EntityConfigurations;

namespace MiniUrl.Association.Infrastructure
{
    public class AssociationContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }

        public AssociationContext(DbContextOptions<AssociationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new AddressEntityTypeConfiguration());
        }
    }

    public class AssociationDesignFactory : IDesignTimeDbContextFactory<AssociationContext>
    {
        public AssociationContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AssociationContext>()
                .UseNpgsql("Host=localhost;Database=Associations;Username=postgres;Password=postgres");

            return new(optionsBuilder.Options);
        }
    }
}
