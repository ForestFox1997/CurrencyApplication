using MediatR;
using FinanceService.Application.GetUserCurrencies;

namespace FinanceService.Application.Queries
{
    public record GetUserCurrenciesQuery(Guid UserId) : IRequest<List<CurrencyDto>>;
}
