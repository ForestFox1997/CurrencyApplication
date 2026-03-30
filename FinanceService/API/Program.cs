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

builder.Services.AddControllers();
builder.Services.AddDbContext<FinanceDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IUserFavoriteRepository, UserFavoriteRepository>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(typeof(AddFavoriteCurrencyCommand).Assembly));
builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(typeof(RemoveFavoriteCurrencyCommand).Assembly));

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
    config.Title = "Finance API";
    config.Version = "v1";
    config.Description = "API для работы с валютами";
});

builder.WebHost.UseUrls("http://0.0.0.0:8080");

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapScalarApiReference(options =>
{
    options.OpenApiRoutePattern = "openapi";
    options.Title = "FinanceService API";
});

app.UseOpenApi(options => options.Path = "openapi");

app.Run();