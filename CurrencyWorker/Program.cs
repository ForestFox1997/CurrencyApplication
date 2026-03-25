using CurrencyWorker;
using CurrencyWorker.Infrastructure;
using CurrencyWorker.Services;
using Microsoft.EntityFrameworkCore;
using System.Text;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddHttpClient();

builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<ICurrencyUpdater, CurrencyUpdater>();

var host = builder.Build();
host.Run();
