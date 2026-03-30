using FinanceService.Application.Commands;
using FinanceService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace FinanceService.API
{
    [ApiController]
    [Route("api/currencies")]
    public class FinanceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FinanceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Получить список валют и их идентификаторы
        /// </summary>
        [HttpGet("currencynames")]
        public async Task<ActionResult> GetCurrencyIds()
        {
            var currencyNames = await _mediator.Send(new GetCurrencyIdsCommand());

            return Ok(currencyNames);
        }

        /// <summary>
        /// Предоставить курсы валют юзера
        /// </summary>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetCurrencies()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _mediator.Send(new GetUserCurrenciesQuery(userId));

            return Ok(result);
        }

        /// <summary>
        /// Сделать валюту избранной для пользователя
        /// </summary>
        [Authorize]
        [HttpPost("favorites")]
        public async Task<ActionResult> AddFavorites([Required] Guid currencyId)
        {
            var userId = GetUserId();

            var command = new AddFavoriteCurrencyCommand(userId, currencyId);

            var result = await _mediator.Send(command);

            return result.Success ? Ok() : BadRequest(result.Error);
        }

        /// <summary>
        /// Убрать валюту из избранного пользователя
        /// </summary>
        [Authorize]
        [HttpDelete("favorites/{currencyId}")]
        public async Task<ActionResult> RemoveFavorites(Guid currencyId)
        {
            var userId = GetUserId();

            var command = new RemoveFavoriteCurrencyCommand(userId, currencyId);
            var result = await _mediator.Send(command);

            return result.Success ? Ok() : NotFound();
        }

        private Guid GetUserId()
        {
            var idClaim = User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

            return Guid.Parse(idClaim!.Value);
        }
    }
}
