using FinanceService.Domain.Entities;

namespace FinanceService.Application.Interfaces
{
    public interface ICurrencyRepository
    {
        Task<IList<Currency>> GetByIdsAsync(IList<Guid> ids, CancellationToken cancellationToken = default);

        Task<IList<Currency>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<Currency?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
