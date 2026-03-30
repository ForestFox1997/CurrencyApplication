using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using MediatR;
using Moq;
using UserService.API;
using UserService.Application.Common;
using UserService.Application.User.Commands;

namespace UserService.Tests
{
    public class UserControllerTests
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly UserController controller;

        public UserControllerTests()
        {
            mediatorMock = new Mock<IMediator>();
            controller = new UserController(mediatorMock.Object);
        }

        [Fact]
        public async Task Register_Should_Call_Mediator()
        {
            var command = new RegisterUserCommand("user", "password");

            mediatorMock.Setup(x => x.Send(command, default)).Returns(Task.FromResult(new Result { Success = true }));

            var result = await controller.Register(command);

            result.Should().BeOfType<OkObjectResult>();

            mediatorMock.Verify(x => x.Send(command, default),Times.Once);
        }

        [Fact]
        public async Task Login_Should_Return_Ok_When_Success()
        {
            var command = new LoginUserCommand("name", "password");

            var response = new Result { Success = true, Message = "token" };

            mediatorMock.Setup(x => x.Send(command, default)).ReturnsAsync(response);

            var result = await controller.Login(command);

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Login_Should_Return_Unauthorized_When_Failed()
        {
            var command = new LoginUserCommand("user", "password");

            var response = new Result { Success = false, Error = "Invalid credentials" };

            mediatorMock.Setup(x => x.Send(command, default)).ReturnsAsync(response);

            var result = await controller.Login(command);

            result.Should().BeOfType<UnauthorizedObjectResult>();
        }

        [Fact]
        public async Task TestAuthorize_Should_Return_Ok_When_User_Exists()
        {
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, "user-id") };

            var identity = new ClaimsIdentity(claims, "TestAuth");

            var user = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } };

            var result = await controller.GetUserLogin();

            result.Should().BeOfType<OkObjectResult>();
        }
    }
}