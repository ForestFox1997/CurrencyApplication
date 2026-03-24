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
            var user = await _repository.GetByNameAsync(request.Name);
            if (user == null)
            {
                return new Result { Success = false, Error = "Неудачная попытка аутентификации" };
            }

            var passwordIsCorrect = _passwordHasher.Verify(request.Password, user.PasswordHash!);
            if (!passwordIsCorrect)
            {
                return new Result { Success = false, Error = "Неудачная попытка аутентификации" };
            }

            return new Result { Success = true,
                Message = $"Удачная аутентификация; Token: {_jwtProvider.GenerateToken(user)}" };
        }
    }
}
