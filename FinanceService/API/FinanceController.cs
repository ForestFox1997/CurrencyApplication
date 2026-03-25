using FinanceService.Application.Commands;
using FinanceService.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinanceService.API
{
    [ApiController]
    public class FinanceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FinanceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
        [HttpGet("/currencies")]
        public async Task<ActionResult> GetCurrencies()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var result = await _mediator.Send(new GetUserCurrenciesQuery(userId));

            return Ok(result);
        }

        [Authorize]
        [HttpPost("/favorites")]
        public async Task<ActionResult> AddFavorites(Guid currencyId)
        {
            var userId = GetUserId();

            var command = new AddFavoriteCurrencyCommand(userId, currencyId);

            var result = await _mediator.Send(command);

            return result.Success ? Ok() : BadRequest(result.Error);
        }

        [Authorize]
        [HttpDelete("/favorites/{currencyId}")]
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
