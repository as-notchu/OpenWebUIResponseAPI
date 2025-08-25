namespace OpenWebUiProxyRApi.Models;

public sealed class ResponsesDto
{
    public string? Id { get; set; }

    public string? Object { get; set; }

    [global::System.Text.Json.Serialization.JsonPropertyName("created_at")]
    public long? CreatedAt { get; set; }

    public string? Model { get; set; }

    public List<MessageDto>? Output { get; set; }

    public UsageDto? Usage { get; set; }
}

public sealed class MessageDto
{
    public string? Role { get; set; }
    public List<ContentPart>? Content { get; set; }
}

public sealed class ContentPart
{
    public string? Type { get; set; }
    public string? Text { get; set; }
}

public sealed class UsageDto
{
    [global::System.Text.Json.Serialization.JsonPropertyName("input_tokens")]
    public int? InputTokens { get; set; }

    [global::System.Text.Json.Serialization.JsonPropertyName("output_tokens")]
    public int? OutputTokens { get; set; }

    [global::System.Text.Json.Serialization.JsonPropertyName("total_tokens")]
    public int? TotalTokens { get; set; }
}