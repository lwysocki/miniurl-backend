using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniUrl.KeyManager.Domain.Models;

namespace MiniUrl.KeyManager.Infrastructure.EntityConfigurations
{
    public class ConfigurationEntityTypeConfiguration : IEntityTypeConfiguration<Configuration>
    {
        public void Configure(EntityTypeBuilder<Configuration> builder)
        {
            // Todo: define common properties once?
            builder.ToTable(typeof(Configuration).Name);
            builder.HasKey(p => p.Key);
            builder.Property(p => p.Key).ValueGeneratedNever();
            builder.Property(p => p.Value).IsRequired();
        }
    }
}
