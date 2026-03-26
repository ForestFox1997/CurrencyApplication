using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using MediatR;
using Moq;
using FinanceService.API;
using FinanceService.Application.Commands;
using FinanceService.Application.Queries;
using FinanceService.Common;
using FinanceService.Application.GetUserCurrencies;

namespace FinanceService.Tests
{
    public class FinanceControllerTests
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly FinanceController controller;

        public FinanceControllerTests()
        {
            mediatorMock = new Mock<IMediator>();
            controller = new FinanceController(mediatorMock.Object);
        }

        private void SetUser(Guid userId)
        {
            var claims = new[]
            {
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
                userId.ToString())
            };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var user = new ClaimsPrincipal(identity);

            controller.ControllerContext =
                new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };
        }

        [Fact]
        public async Task GetCurrencies_Should_Return_Ok_With_Result()
        {
            // Arrange
            var userId = Guid.NewGuid();
            SetUser(userId);

            var currencies = new List<CurrencyDto>() { new() { Name = "EUR", Rate = 100 } };

            mediatorMock
                .Setup(x => x.Send(It.IsAny<GetUserCurrenciesQuery>(), default)).ReturnsAsync(() => currencies);

            var result = await controller.GetCurrencies();

            result.Should().BeOfType<OkObjectResult>();
            var okResult = result as OkObjectResult;
            okResult!.Value.Should().BeEquivalentTo(currencies);
        }

        [Fact]
        public async Task AddFavorites_Should_Return_Ok_When_Success()
        {
            var userId = Guid.NewGuid();
            SetUser(userId);
            var currencyId = Guid.NewGuid();

            mediatorMock
                .Setup(x => x.Send(It.IsAny<AddFavoriteCurrencyCommand>(), default))
                .ReturnsAsync(new Result { Success = true });

            var result = await controller.AddFavorites(currencyId);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task AddFavorites_Should_Return_BadRequest_When_Failed()
        {
            var userId = Guid.NewGuid();
            SetUser(userId);
            var currencyId = Guid.NewGuid();

            mediatorMock
                .Setup(x => x.Send(It.IsAny<AddFavoriteCurrencyCommand>(), default))
                .ReturnsAsync(new Result { Success = false, Error = "error" });

            var result = await controller.AddFavorites(currencyId);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task RemoveFavorites_Should_Return_Ok_When_Success()
        {
            var userId = Guid.NewGuid();
            SetUser(userId);
            var currencyId = Guid.NewGuid();

            mediatorMock
                .Setup(x => x.Send(It.IsAny<RemoveFavoriteCurrencyCommand>(), default))
                .ReturnsAsync(new Result { Success = true });

            var result = await controller.RemoveFavorites(currencyId);

            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task RemoveFavorites_Should_Return_NotFound_When_Failed()
        {
            var userId = Guid.NewGuid();
            SetUser(userId);
            var currencyId = Guid.NewGuid();

            mediatorMock
                .Setup(x => x.Send(It.IsAny<RemoveFavoriteCurrencyCommand>(), default))
                .ReturnsAsync(new Result { Success = false });

            var result = await controller.RemoveFavorites(currencyId);

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
