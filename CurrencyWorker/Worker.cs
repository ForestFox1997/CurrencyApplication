using CurrencyWorker.Services;

namespace CurrencyWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _provider;

    public Worker(IServiceProvider serviceProvider, ILogger<Worker> logger)
    {
        _provider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CurrencyWorker запущен");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _provider.CreateScope();

                var updater = scope.ServiceProvider.GetRequiredService<ICurrencyUpdater>();

                await updater.UpdateAsync(stoppingToken);

                _logger.LogInformation("Курсы валют обновлены");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при попытке обновления курсов валют");
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
