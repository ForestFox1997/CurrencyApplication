using System.Text.Json.Nodes;
using Scalar.AspNetCore;
using Gateway.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy().LoadFromMemory(ReverseProxyConfig.GetRoutes(), ReverseProxyConfig.GetClusters());

builder.Services.AddControllers();

builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("Default")!, name: "postgres");

var app = builder.Build();

app.UseRouting();

app.MapControllers();

app.MapHealthChecks("/health");

app.MapScalarApiReference(options =>
{
    options.Title = "Currency Gateway API";
    options.AddDocument(documentName: "finance", title: "Finance API", routePattern: "/finance/openapi");
    options.AddDocument(documentName: "user", title: "User API", routePattern: "/user/openapi");
});

// Редирект на страницу scalar
app.MapGet("/", async () =>
{
    return Results.Redirect("/scalar");
});

// HACK !! Маршрутизации для поддержки вызова API методов из веб-интерфейса scalar
app.MapGet("/finance/openapi", async () =>
{
    using var http = new HttpClient();

    var json = await http.GetStringAsync("http://finance-service:8080/openapi");
    var node = JsonNode.Parse(json)!;

    node["servers"] = new JsonArray { new JsonObject { ["url"] = "/finance" } };

    return Results.Content(node.ToJsonString(), "application/json");
});

app.MapGet("/user/openapi", async () =>
{
    using var http = new HttpClient();

    var json = await http.GetStringAsync("http://user-service:8080/openapi");
    var node = JsonNode.Parse(json)!;

    node["servers"] = new JsonArray { new JsonObject { ["url"] = "/user" } };

    return Results.Content(node.ToJsonString(), "application/json");
});

app.MapReverseProxy();

app.Run();