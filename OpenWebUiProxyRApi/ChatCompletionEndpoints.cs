using System.ClientModel;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Responses;
using OpenWebUiProxyRApi.Models;

namespace OpenWebUiProxyRApi;

public static class ChatCompletionEndpoints
{
    [Experimental("OPENAI001")]
    public static void MapChatCompletionEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/chat/completions", async (ChatCompletionRequest request, ResponseService service) =>
        {
            var modelName = request.Model;
            Console.WriteLine(request.ReasoningEffort);
            var response = await service.GetResponseAsync(modelName, request.Messages, request.ReasoningEffort);
            var m = response.OutputItems[0] as MessageResponseItem;
            var chatCompletion = new
            {
                id = response.Id,
                @object = "chat.completion",
                created = response.CreatedAt,
                model = modelName,
                choices = new[]
                {
                    new {
                        index = 0,
                        message = new { role = "assistant", content = response.GetOutputText() },
                        logprobs = (object?)null,
                        finish_reason = response.Status
                    }
                },
                usage = new
                {
                    prompt_tokens = response.Usage.InputTokenCount,
                    completion_tokens = response.Usage.OutputTokenCount,
                    total_tokens = response.Usage.TotalTokenCount,
                } 
            };

            return Results.Json(chatCompletion);
        }).RequireAuthorization();
    }
}