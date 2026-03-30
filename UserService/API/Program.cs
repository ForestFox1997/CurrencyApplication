using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using UserService.Application;
using UserService.Application.Common.Interfaces;
using UserService.Application.User.Commands;
using UserService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "v1";
    config.Title = "User API";
    config.Version = "v1";
    config.Description = "API для регистрации, аутентификации и авторизации пользователей";
});

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly));
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddControllers();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
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

builder.WebHost.UseUrls("http://0.0.0.0:8080");

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(options => options.OpenApiRoutePattern = "openapi");
    app.MapControllers();
    app.UseOpenApi(options => options.Path = "openapi");
}

app.UseAuthentication();
app.UseAuthorization();

app.Run();