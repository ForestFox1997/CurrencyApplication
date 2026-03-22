namespace UserService.Application
{
    /// <summary>
    /// Предоставляет возможность извлечения хэша из тела пароля и проверки,
    /// соответствует ли переданный пароль хэшу
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Получить хэш из тела пароля
        /// </summary>
        string Hash(string password);

        /// <summary>
        /// Показывает, соответствует ли переданный пароль хэшу
        /// </summary>
        bool Verify(string password, string hash);
    }
}
