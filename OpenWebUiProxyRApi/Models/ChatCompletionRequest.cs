using System.Text.Json.Serialization;

namespace OpenWebUiProxyRApi.Models;

public class ChatCompletionRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("messages")]
    public List<ChatMessage> Messages { get; set; }

    [JsonPropertyName("reasoning_effort")] public string ReasoningEffort { get; set; } = "None";
}