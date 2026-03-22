namespace UserService.Application.Common.Interfaces
{
    /// <summary>
    /// Провайдер для работы с JWT токенами
    /// </summary>
    public interface IJwtProvider
    {
        string GenerateToken(Domain.User user);
    }
}
