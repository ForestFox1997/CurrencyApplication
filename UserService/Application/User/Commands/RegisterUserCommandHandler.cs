using System.Text.RegularExpressions;
using MediatR;
using UserService.Application.Common;

namespace UserService.Application.User.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result>
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _hasher;

        public RegisterUserCommandHandler(IUserRepository repository, IPasswordHasher passwordHasher)
        {
            _repository = repository;
            _hasher = passwordHasher;
        }

        public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (!Regex.IsMatch(request.Name, @"^[A-Za-z0-9_]{6,20}$"))
            {
                return new Result { Success = false, Error = "Ник пользователя не соответствует требованиям" };
            }

            if (!Regex.IsMatch(request.Password, @"^[A-Za-z0-9_$%^&*]{10,20}$"))
            {
                return new Result { Success = false, Error = "Пароль пользователя не соответствует требованиям" };
            }

            var existing = await _repository.GetByNameAsync(request.Name);
            if (existing != null)
            {
                return new Result { Success = false, Error = "Пользователя с таким ником уже зарегистрирован" };
            }

            var passwordHash = _hasher.Hash(request.Password);

            var user = new Domain.User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                PasswordHash = passwordHash,
            };

            await _repository.AddAsync(user);

            return new Result { Success = true, Message = "Пользователь успешно зарегистрирован" };
        }
    }
}
