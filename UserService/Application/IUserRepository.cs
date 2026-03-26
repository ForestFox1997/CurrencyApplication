using UserService.Domain;

namespace UserService.Application
{
    /// <summary>
    /// Репозиторий для работы с пользователями
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Добавить пользователя
        /// </summary>
        Task AddAsync(Domain.User user);

        /// <summary>
        /// Найти пользователя по его имени
        /// </summary>
        Task<Domain.User?> GetByNameAsync(string name);
    }
}
