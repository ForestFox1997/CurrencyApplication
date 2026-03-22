using Microsoft.EntityFrameworkCore;
using UserService.Domain;

namespace UserService.Infrastructure
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> dbContextOptions)
        {
            _ = dbContextOptions;
        }

        public DbSet<User> Users => Set<User>();
    }
}
