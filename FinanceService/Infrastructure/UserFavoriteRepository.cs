using FinanceService.Application.Interfaces;
using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceService.Infrastructure
{
    public class UserFavoriteRepository : IUserFavoriteRepository
    {
        private readonly FinanceDbContext _context;

        public UserFavoriteRepository(FinanceDbContext financeDbContext)
        {
            _context = financeDbContext;
        }

        public async Task AddAsync(UserFavoriteCurrency favorite, CancellationToken cancellationToken = default)
        {
            await _context.Favorites.AddAsync(favorite, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IList<UserFavoriteCurrency>> GetByUserIdAsync(
            Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Favorites.Where(c => c.UserId == userId).ToListAsync(cancellationToken);
        }

        public async Task<bool> RemoveAsync(Guid userId, Guid currencyId, CancellationToken cancellationToken = default)
        {
            var removedCurrency = await _context.Favorites.FirstOrDefaultAsync(
                c => c.UserId == userId && c.CurrencyId == currencyId, cancellationToken);

            if (removedCurrency == null)
            {
                return false;
            }

            _context.Favorites.Remove(removedCurrency);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> IsCurrencyExists(Guid currencyId)
        {
            return await _context.Currencies.CountAsync(c => c.Id == currencyId) > 0;
        }
    }
}
