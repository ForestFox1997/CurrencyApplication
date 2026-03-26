using FinanceService.Application.GetUserCurrencies;
using FinanceService.Application.Interfaces;
using MediatR;

namespace FinanceService.Application.Queries
{
    public class GetUserCurrenciesHandler : IRequestHandler<GetUserCurrenciesQuery, List<CurrencyDto>>
    {
        private readonly IUserFavoriteRepository _favoriteRepository;
        private readonly ICurrencyRepository _currencyRepository;

        public GetUserCurrenciesHandler(
            IUserFavoriteRepository favoriteRepository, ICurrencyRepository currencyRepository)
        {
            _favoriteRepository = favoriteRepository;
            _currencyRepository = currencyRepository;
        }

        public async Task<List<CurrencyDto>> Handle(
            GetUserCurrenciesQuery request,
            CancellationToken cancellationToken)
        {
            var favorites = await _favoriteRepository
                .GetByUserIdAsync(
                    request.UserId,
                    cancellationToken);

            var currencyIds = favorites
                .Select(x => x.CurrencyId)
                .ToList();

            var currencies = await _currencyRepository
                .GetByIdsAsync(
                    currencyIds,
                    cancellationToken);

            return currencies
                .Select(c => new CurrencyDto
                {
                    //Id = c.Id,
                    Name = c.Name,
                    Rate = c.Rate
                })
                .ToList();
        }
    }
}
