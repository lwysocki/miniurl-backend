using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniUrl.KeyManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniUrl.KeyManager.Infrastructure.EntityConfigurations
{
    public class KeyGeneratorConfigurationEntityTypeConfiguration : IEntityTypeConfiguration<KeyGeneratorConfiguration>
    {
        public void Configure(EntityTypeBuilder<KeyGeneratorConfiguration> builder)
        {
            // Todo: define common properties once?
            builder.ToTable(typeof(Configuration).Name);
            builder.HasKey(p => p.Key);
            builder.Property(p => p.Key).ValueGeneratedNever();
            builder.Property(p => p.Value).IsRequired();
            builder.HasBaseType<Configuration>();
        }
    }
}
