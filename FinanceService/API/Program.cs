using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using Scalar.AspNetCore;
using FinanceService.Application.Commands;
using FinanceService.Application.Interfaces;
using FinanceService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IUserFavoriteRepository, UserFavoriteRepository>();
//builder.Services.AddDbContext<CurrencyDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(typeof(AddFavoriteCurrencyCommand).Assembly));
builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(typeof(RemoveFavoriteCurrencyCommand).Assembly));

builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz"))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "v1";
    config.PostProcess = doc =>
    {
        doc.Info = new OpenApiInfo
        {
            Title = "My API",
            Version = "v1",
            Description = "Minimal NSwag-generated OpenAPI"
        };
    };
    // config.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
    // {
    //     Type = OpenApiSecuritySchemeType.ApiKey,
    //     Name = "Authorization",
    //     In = OpenApiSecurityApiKeyLocation.Header,
    //     Description = "Type: Bearer {token}"
    // });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(options => options.OpenApiRoutePattern = "openapi");
    app.MapControllers();
    app.UseOpenApi(options => options.Path = "openapi");
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
