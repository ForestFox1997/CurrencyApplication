using Microsoft.AspNetCore.Identity;
using UserService.Application;
using UserService.Domain;

namespace UserService.Infrastructure
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasher<User> _hasher;

        public PasswordHasher()
        {
            _hasher = new PasswordHasher<User>();
        }

        public string Hash(string password)
        {
            return _hasher.HashPassword(null!, password);
        }

        public bool Verify(string password, string hash)
        {
            var result = _hasher.VerifyHashedPassword(null!, password, hash);
            return result == PasswordVerificationResult.Success;
        }
    }
}
