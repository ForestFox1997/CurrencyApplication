namespace FinanceService.Domain.Entities
{
    public class UserFavoriteCurrency
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Идентификатор валюты
        /// </summary>
        public Guid CurrencyId { get; set; }
    }
}
