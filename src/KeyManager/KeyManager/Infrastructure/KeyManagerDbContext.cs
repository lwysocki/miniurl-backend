using Microsoft.EntityFrameworkCore;
using MiniUrl.KeyManager.Domain.Models;
using MiniUrl.KeyManager.Infrastructure.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniUrl.KeyManager.Infrastructure
{
    public class KeyManagerDbContext : DbContext
    {
        public DbSet<KeyManagerConfiguration> KeyManagerConfigurations { get; set; }
        public DbSet<Key> Keys { get; set; }

        public KeyManagerDbContext(DbContextOptions<KeyManagerDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new KeyEntityTypeConfiguration());
            builder.ApplyConfiguration(new KeyManagerConfigurationEntityTypeConfiguration());
        }
    }
}
