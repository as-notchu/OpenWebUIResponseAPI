using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using OpenWebUiProxyRApi;

var builder = WebApplication.CreateBuilder(args);


var modelsJsonPath = Path.Combine(builder.Environment.ContentRootPath, "models.json");
var modelsJsonContent = await File.ReadAllTextAsync(modelsJsonPath);
builder.Services.AddSingleton(new ModelsService(modelsJsonContent));


builder.Services.AddAuthentication("StaticBearer")
    .AddScheme<StaticBearerOptions, StaticBearerHandler>("StaticBearer", options =>
    {
        options.Token = builder.Configuration["JWT"] ?? throw new InvalidOperationException("Auth:Token not configured");
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.MapGet("/api/models", (ModelsService models) => Results.Content(models.JsonContent, "application/json"))
    .RequireAuthorization();
    
app.MapGet("/v1/models", (ModelsService models) => Results.Content(models.JsonContent, "application/json"))
    .RequireAuthorization();

    app.Run();