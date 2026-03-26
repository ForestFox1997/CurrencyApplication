namespace UserService.Domain
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Имя / ник / логин
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Хэш пароля
        /// </summary>
        public string? PasswordHash { get; set; }

        /// <summary>
        /// Коллекция любимых валют пользователя
        /// </summary>
        public ICollection<FavoriteCurrency> Favorites { get; set; } = [];
    }
}
