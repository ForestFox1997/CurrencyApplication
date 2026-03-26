using FinanceService.Domain.Entities;

namespace FinanceService.Application.Interfaces
{
    public interface IUserFavoriteRepository
    {
        Task<IList<UserFavoriteCurrency>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

        Task AddAsync(UserFavoriteCurrency favorite, CancellationToken cancellationToken = default);

        Task<bool> RemoveAsync(Guid userId, Guid currencyId, CancellationToken cancellationToken = default);

        Task<bool> IsCurrencyExists(Guid currencyId);
    }
}
