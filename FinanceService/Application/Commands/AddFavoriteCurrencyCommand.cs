using FinanceService.Common;
using MediatR;

namespace FinanceService.Application.Commands
{
    public record AddFavoriteCurrencyCommand(Guid UserId, Guid CurrencyId) : IRequest<Result>;
}
