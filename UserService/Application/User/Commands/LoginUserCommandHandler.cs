using MediatR;
using UserService.Application.Common;
using UserService.Application.Common.Interfaces;

namespace UserService.Application.User.Commands
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result>
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtProvider _jwtProvider;

        public LoginUserCommandHandler(IUserRepository repository,
            IPasswordHasher passwordHasher, IJwtProvider jwtProvider)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _jwtProvider = jwtProvider;
        }

        public async Task<Result> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
