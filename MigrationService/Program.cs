using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserService.Infrastructure;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var connectionString = context.Configuration.GetConnectionString("Default");

        services.AddDbContext<UserDbContext>(options =>
            options.UseNpgsql(connectionString));
    })
    .Build();

using var scope = host.Services.CreateScope();

var services = scope.ServiceProvider;

try
{
    Console.WriteLine("Применяются миграции...");

    var userDb = services.GetRequiredService<UserDbContext>();

    await userDb.Database.MigrateAsync();

    Console.WriteLine("Миграции успешно применены...");
}
catch (Exception ex)
{
    Console.WriteLine("Ошибка при применении миграции: {0}", ex);
    throw;
}
