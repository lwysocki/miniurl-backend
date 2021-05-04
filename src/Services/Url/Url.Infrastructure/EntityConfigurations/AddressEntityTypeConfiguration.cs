using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniUrl.Url.Domain.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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
