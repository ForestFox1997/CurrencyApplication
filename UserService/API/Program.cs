using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using Scalar.AspNetCore;
using System.Text;
using UserService.Application;
using UserService.Application.Common.Interfaces;
using UserService.Application.User.Commands;
using UserService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApiDocument(config =>
{
    // config.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
    // {
    //     Type = OpenApiSecuritySchemeType.ApiKey,
    //     Name = "Authorization",
    //     In = OpenApiSecurityApiKeyLocation.Header,
    //     Description = "Type: Bearer {token}"
    // });
});

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddMediatR(config =>
    config.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly));
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
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

builder.WebHost.UseUrls("http://0.0.0.0:8080");

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(options => options.OpenApiRoutePattern = "openapi");
    app.MapControllers();
    app.UseOpenApi(options => options.Path = "openapi");
}

app.UseAuthentication();
app.UseAuthorization();

app.Run();
