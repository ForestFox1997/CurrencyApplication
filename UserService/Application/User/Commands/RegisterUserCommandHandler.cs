using MediatR;

namespace UserService.Application.User.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _hasher;

        public RegisterUserCommandHandler(IUserRepository repository, IPasswordHasher passwordHasher)
        {
            _repository = repository;
            _hasher = passwordHasher;

        }

        public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByNameAsync(request.Name);
            if (existing != null)
                throw new Exception("User already exists");

            var passwordHash = _hasher.Hash(request.Password);

            var user = new Domain.User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                PasswordHash = passwordHash,
            };

            await _repository.AddAsync(user);
        }
    }
}
