using Microsoft.AspNetCore.Authentication;

namespace OpenWebUiProxyRApi;

public class StaticBearerOptions : AuthenticationSchemeOptions
{
    public string Token { get; set; } = string.Empty;
}