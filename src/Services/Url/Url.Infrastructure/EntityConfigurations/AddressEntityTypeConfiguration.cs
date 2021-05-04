using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniUrl.Url.Domain.Model;

namespace MiniUrl.Url.Infrastructure.EntityConfigurations
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
