using FinanceService.Application.Interfaces;
using FinanceService.Common;
using FinanceService.Domain.Entities;
using MediatR;

namespace FinanceService.Application.Commands
{
    public class AddFavoriteCurrencyHandler : IRequestHandler<AddFavoriteCurrencyCommand, Result>
    {
        private readonly IUserFavoriteRepository _repository;

        public AddFavoriteCurrencyHandler(IUserFavoriteRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(AddFavoriteCurrencyCommand request, CancellationToken cancellationToken)
        {
            if (!await _repository.IsCurrencyExists(request.CurrencyId))
            {
                return new Result {Success = false, Error = "Валюты с указанным id не существует" };
            }

            var favorite = new UserFavoriteCurrency
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                CurrencyId = request.CurrencyId
            };

            await _repository.AddAsync(favorite, cancellationToken);

            return new Result { Success = true };
        }
    }
}
