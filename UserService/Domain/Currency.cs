namespace UserService.Domain
{
    /// <summary>
    /// Курс валюты
    /// </summary>
    public class Currency
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Курс
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// Избранные валюты
        /// </summary>
        public ICollection<FavoriteCurrency> Favorites { get; set; } = [];
    }
}
