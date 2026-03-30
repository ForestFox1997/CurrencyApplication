namespace FinanceService.Domain.Entities
{
    /// <summary>
    /// Наименование валюты
    /// </summary>
    public class CurrencyName
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; }
    }
}
