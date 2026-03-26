namespace UserService.Domain
{
    /// <summary>
    /// Любимая валюта пользователя
    /// </summary>
    public class FavoriteCurrency
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Идентификатор валюты
        /// </summary>
        public int CurrencyId { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public User User { get; set; } = null!;
        
    }
}
