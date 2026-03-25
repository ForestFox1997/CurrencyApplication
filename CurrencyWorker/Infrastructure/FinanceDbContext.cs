using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CurrencyWorker.Models;

namespace CurrencyWorker.Infrastructure
{
    internal class FinanceDbContext : DbContext
    {
        public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options)
        {

        }

        public DbSet<Currency> Currencies => Set<Currency>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Currency>(entity =>
            {
                entity.ToTable("currencies");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(x => x.Rate)
                    .HasColumnType("numeric(18,6)")
                    .IsRequired();

                entity.HasIndex(x => x.Name)
                    .IsUnique();
            });
        }
    }
}
