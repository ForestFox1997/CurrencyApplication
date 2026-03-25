using FinanceService.Application.Interfaces;
using FinanceService.Common;
using MediatR;

namespace FinanceService.Application.Commands
{
    public class RemoveFavoriteCurrencyHandler : IRequestHandler<RemoveFavoriteCurrencyCommand, Result>
    {
        private readonly IUserFavoriteRepository _repository;

        public RemoveFavoriteCurrencyHandler(IUserFavoriteRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(RemoveFavoriteCurrencyCommand request, CancellationToken cancellationToken)
        {
            var removed = await _repository.RemoveAsync(
                request.UserId,
                request.CurrencyId,
                cancellationToken);

            if (!removed)
                return new Result { Success = false, Error = "Избранная валюта не найдена" };

            return new Result { Success = true, Message = "Избранная валюта удалена"};
        }
    }
}
