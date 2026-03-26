using MediatR;
using UserService.Application.Common;

namespace UserService.Application.User.Commands
{
    public record LoginUserCommand(string Name, string Password) : IRequest<Result>;
}
