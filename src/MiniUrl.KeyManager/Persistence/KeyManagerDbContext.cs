using Microsoft.EntityFrameworkCore;
using MiniUrl.KeyManager.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniUrl.KeyManager.Persistence
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
            base.OnModelCreating(builder);

            builder.Entity<Key>().ToTable("Keys");
            builder.Entity<Key>().HasKey(p => p.Id);
            builder.Entity<Key>().Property(p => p.Id).IsRequired();
            builder.Entity<Key>().Property(p => p.State).IsRequired();

            // Todo: define common properties once?
            builder.Entity<KeyManagerConfiguration>().ToTable(typeof(Configuration).Name);
            builder.Entity<KeyManagerConfiguration>().HasKey(p => p.Key);
            builder.Entity<KeyManagerConfiguration>().Property(p => p.Key).ValueGeneratedNever();
            builder.Entity<KeyManagerConfiguration>().Property(p => p.Value).IsRequired();
            builder.Entity<KeyManagerConfiguration>().HasBaseType<Configuration>();
        }
    }
}
