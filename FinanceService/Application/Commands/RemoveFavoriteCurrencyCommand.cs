using FinanceService.Common;
using MediatR;

namespace FinanceService.Application.Commands
{
    public record RemoveFavoriteCurrencyCommand(Guid UserId, Guid CurrencyId) : IRequest<Result>;
}
