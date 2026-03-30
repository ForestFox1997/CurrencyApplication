using MediatR;
using FinanceService.Application.Interfaces;
using FinanceService.Domain.Entities;
using FinanceService.Common;

namespace FinanceService.Application.Commands
{
    public class GetCurrencyIdsHandler : IRequestHandler<GetCurrencyIdsCommand, IList<CurrencyName>>
    {
        private readonly ICurrencyRepository _repository;

        public GetCurrencyIdsHandler(ICurrencyRepository repository)
        {
            _repository = repository;
        }

        public async Task<IList<CurrencyName>> Handle(
            GetCurrencyIdsCommand request, CancellationToken cancellationToken)
        {
            var allCurrencies = await _repository.GetAllAsync(cancellationToken);

            var currencyInfoList = allCurrencies.Select(c => new CurrencyName { Id = c.Id, Name = c.Name }).ToList();

            return currencyInfoList;
        }
    }
}
