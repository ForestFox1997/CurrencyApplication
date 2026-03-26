namespace FinanceService.Common
{
    /// <summary>
    /// Результат работы FinanceService
    /// </summary>
    public class Result
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Error { get; set; }
    }
}
