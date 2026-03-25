using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Infrastructure
{
    public class FinanceDbContext : DbContext
    {
        public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options)
        {

        }

        public DbSet<Currency> Currencies => Set<Currency>();

        public DbSet<UserFavoriteCurrency> Favorites => Set<UserFavoriteCurrency>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureCurrency(builder);

            ConfigureFavorites(builder);
        }

        private static void ConfigureCurrency(ModelBuilder builder)
        {
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
            });
        }

        private static void ConfigureFavorites(ModelBuilder builder)
        {
            builder.Entity<UserFavoriteCurrency>(entity =>
            {
                entity.ToTable("favorites");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.UserId)
                    .IsRequired();

                entity.Property(x => x.CurrencyId)
                    .IsRequired();

                entity.HasIndex(x => new
                {
                    x.UserId,
                    x.CurrencyId
                })
                .IsUnique();
            });
        }
    }
}
