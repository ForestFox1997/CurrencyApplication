using MediatR;
using UserService.Application.Common;

namespace UserService.Application.User.Commands
{
    public record RegisterUserCommand(string Name, string Password) : IRequest<Result>;
}
