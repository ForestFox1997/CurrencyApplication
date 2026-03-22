using MediatR;

namespace UserService.Application.User.Commands
{
    public record RegisterUserCommand(string Name, string Password) : IRequest;
}
