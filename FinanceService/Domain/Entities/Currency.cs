namespace FinanceService.Domain.Entities
{
    /// <summary>
    /// Валюта
    /// </summary>
    public class Currency
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название валюты
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Курс валюты к российскому рублю
        /// </summary>
        public decimal Rate { get; set; }
    }
}
