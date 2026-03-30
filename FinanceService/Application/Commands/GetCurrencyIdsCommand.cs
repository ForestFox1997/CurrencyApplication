using System.Collections.Generic;
using MediatR;
using FinanceService.Common;
using FinanceService.Domain.Entities;

namespace FinanceService.Application.Commands
{
    public record GetCurrencyIdsCommand() : IRequest<IList<CurrencyName>>;
}
