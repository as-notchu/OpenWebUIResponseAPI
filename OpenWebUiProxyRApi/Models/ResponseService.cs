using System.ClientModel;
using System.ClientModel.Primitives;
using System.Diagnostics.CodeAnalysis;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Responses;
#pragma warning disable OPENAI001

namespace OpenWebUiProxyRApi.Models;

public class ResponseService : IGetResponse
{

    public async Task<ChatCompletion> GetChatCompletion(string model, string message, string effort)
    {
        var chat = new ChatClient(model, Environment.GetEnvironmentVariable("OPEN-AI"));
        var cMessage = new UserChatMessage(message);
        var messages = new List<OpenAI.Chat.ChatMessage>() { cMessage };

        ChatCompletionOptions a = new ChatCompletionOptions()
        {
            ReasoningEffortLevel = MapEffort(effort)

        };

        var b = await chat.CompleteChatAsync(messages: messages, a, CancellationToken.None);

        return b.Value;
    }

    public async Task<OpenAIResponse> GetResponseAsync(string model, List<ChatMessage> messages, string effort)
    {
        var chat = new OpenAIResponseClient(model, Environment.GetEnvironmentVariable("OPEN-AI"));
        
        var res = MapEffortR(effort);
        var list = new List<ResponseItem>();
        foreach (var m in messages)
        {
            switch (m.Role)
            {
                case "user":
                    list.Add(ResponseItem.CreateUserMessageItem(m.Content));
                    break;
                case "assistant":
                    list.Add(ResponseItem.CreateUserMessageItem(m.Content));
                    break;
                default:
                    list.Add(ResponseItem.CreateSystemMessageItem(m.Content));
                    break;
            }
        }
        var r = await chat.CreateResponseAsync(list,
            new ResponseCreationOptions()
            {
                Tools = { ResponseTool.CreateWebSearchTool() },
                ReasoningOptions = new ResponseReasoningOptions(){ReasoningEffortLevel = res},
            });
        
        return r.Value;
    }

static ChatReasoningEffortLevel MapEffort(string s)
    {
        switch (s.Trim().ToLowerInvariant())
        {
            case "low":  return ChatReasoningEffortLevel.Low;
            case "medium": return ChatReasoningEffortLevel.Medium;
            case "high": return ChatReasoningEffortLevel.High;
            default: return ChatReasoningEffortLevel.Low;
        }
    }
    static ResponseReasoningEffortLevel MapEffortR(string s)
    {
        switch (s.Trim().ToLowerInvariant())
        {
            case "low":  return ResponseReasoningEffortLevel.Low;
            case "medium": return ResponseReasoningEffortLevel.Medium;
            case "high": return ResponseReasoningEffortLevel.High;
            default: return ResponseReasoningEffortLevel.Low;
        }
    }
}