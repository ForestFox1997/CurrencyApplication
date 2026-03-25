using FinanceService.Application.Interfaces;
using FinanceService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace FinanceService.Infrastructure
{
    public class CurrencyRepository : ICurrencyRepository
    {
        private readonly FinanceDbContext _context;

        public CurrencyRepository(FinanceDbContext financeDbContext)
        {
            _context = financeDbContext;
        }

        public async Task<IList<Currency>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Currencies.ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<IList<Currency>> GetByIdsAsync(IList<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await _context.Currencies
                .Where(c => ids.Contains(c.Id))
                .ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<Currency?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Currencies
                .Where(c => c.Name == name)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
