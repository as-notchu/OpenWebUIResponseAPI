using Microsoft.AspNetCore.Http.HttpResults;
using OpenWebUiProxyRApi.Models;

namespace OpenWebUiProxyRApi;

public static class ChatCompletionEndpoints
{
    public static void MapChatCompletionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/chat/completion", (ChatCompletionRequest request) =>
        {
            var modelName = request.Model;
            var userMessage = request.Messages.FirstOrDefault(m => m.Role == "user")?.Content;

            return Results.Ok(new { reply = $"Отве" });
        }).RequireAuthorization();
    }
}