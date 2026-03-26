using Microsoft.EntityFrameworkCore;
using UserService.Domain;

namespace UserService.Infrastructure
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> dbContextOptions) : base(dbContextOptions)
        {
            _ = dbContextOptions;
        }

        public DbSet<User> Users => Set<User>();

        public DbSet<FavoriteCurrency> FavoriteCurrencies => Set<FavoriteCurrency>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureUser(modelBuilder);
            ConfigureFavoriteCurrency(modelBuilder);
        }

        private static void ConfigureUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .ValueGeneratedNever();

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.HasIndex(x => x.Name)
                    .IsUnique();
            });
        }

        private static void ConfigureFavoriteCurrency(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FavoriteCurrency>(entity =>
            {
                entity.ToTable("favorite_currency");

                entity.HasKey(x => new
                {
                    x.UserId,
                    x.CurrencyId
                });

                entity.HasOne(x => x.User)
                    .WithMany(x => x.Favorites)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
