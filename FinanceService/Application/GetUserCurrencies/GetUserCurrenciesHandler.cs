using FinanceService.Application.Interfaces;
using FinanceService.Application.Queries;

namespace FinanceService.Application.GetUserCurrencies
{
    public class GetUserCurrenciesHandler
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IUserFavoriteRepository _favoriteRepository;

        public GetUserCurrenciesHandler(
            ICurrencyRepository currencyRepository, IUserFavoriteRepository favoriteRepository)
        {
            _currencyRepository = currencyRepository;
            _favoriteRepository = favoriteRepository;
        }

        public async Task<List<CurrencyDto>> Handle(
            GetUserCurrenciesQuery request, CancellationToken cancellationToken)
        {
            var favorites = await _favoriteRepository.GetByUserIdAsync(request.UserId);

            var currencyIds = favorites.Select(f => f.CurrencyId);

            var currencies = await _currencyRepository.GetByIdsAsync(currencyIds.ToList());

            return currencies.Select(c => new CurrencyDto { Name = c.Name, Rate = c.Rate }).ToList();
        }
    }
}
