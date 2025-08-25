using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using OpenAI.Responses;
using OpenWebUiProxyRApi;
using OpenWebUiProxyRApi.Models;

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

var value = builder.Configuration["OPEN-AI"];

Environment.SetEnvironmentVariable("OPEN-AI", value, EnvironmentVariableTarget.Process);

builder.Services.AddScoped<ResponseService>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.MapGet("/api/models", (ModelsService models) => Results.Content(models.JsonContent, "application/json"))
    .RequireAuthorization();

#pragma warning disable OPENAI001
app.MapChatCompletionEndpoints();
#pragma warning restore OPENAI001
app.Use(async (context, next) =>
{
    if (context.Request.Path.Equals("/api/chat/completions", StringComparison.OrdinalIgnoreCase)
        && context.Request.Method == HttpMethods.Post)
    {
        context.Request.EnableBuffering(); // позволяет читать поток несколько раз
        using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
        var body = await reader.ReadToEndAsync();
        Console.WriteLine("Raw request body:");
        Console.WriteLine(body);
        context.Request.Body.Position = 0; // вернуть поток, чтобы модельный биндинг мог прочитать тело
    }

    await next();
});
app.Run();