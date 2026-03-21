namespace UserService.Application.Common.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(Domain.User user);
    }
}
