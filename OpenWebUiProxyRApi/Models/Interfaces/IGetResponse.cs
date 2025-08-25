using OpenAI.Chat;
using OpenAI.Responses;
#pragma warning disable OPENAI001

namespace OpenWebUiProxyRApi.Models;

public interface IGetResponse
{
    public Task<ChatCompletion> GetChatCompletion(string model, string message, string effort);
    
    public Task<OpenAIResponse> GetResponseAsync(string model, List<ChatMessage> messages, string effort);
}