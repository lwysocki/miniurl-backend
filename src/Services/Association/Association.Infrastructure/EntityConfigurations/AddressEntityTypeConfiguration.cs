using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniUrl.Association.Domain.Model;

namespace MiniUrl.Association.Infrastructure.EntityConfigurations
{
    class AddressEntityTypeConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Url).HasMaxLength(2048).IsRequired();
        }
    }
}
