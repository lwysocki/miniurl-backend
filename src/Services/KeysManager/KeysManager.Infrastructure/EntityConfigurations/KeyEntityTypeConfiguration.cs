using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniUrl.KeyManager.Domain.Models;

namespace MiniUrl.KeyManager.Infrastructure.EntityConfigurations
{
    public class KeyEntityTypeConfiguration : IEntityTypeConfiguration<Key>
    {
        public void Configure(EntityTypeBuilder<Key> builder)
        {
            builder.ToTable("Keys");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).IsRequired();
            builder.Property(p => p.State).IsRequired();
        }
    }
}
