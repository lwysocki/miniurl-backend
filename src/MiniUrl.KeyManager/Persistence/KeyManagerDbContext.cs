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
        }
    }
}
