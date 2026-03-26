namespace UserService.Application.Common
{
    /// <summary>
    /// Результат работы UserService
    /// </summary>
    public class Result
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Error { get; set; }
    }
}
