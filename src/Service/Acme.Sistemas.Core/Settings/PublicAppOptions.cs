namespace Acme.Sistemas.Core.Settings;

public sealed class PublicAppOptions
{
    public const string SectionName = "PublicApp";

    public string BaseUrl { get; set; } = "https://app.atena.local";
    public string EmailConfirmationPath { get; set; } = "/confirmar-email";

    public string BuildEmailConfirmationUrl(string token)
    {
        var baseUrl = BaseUrl.TrimEnd('/');
        var path = EmailConfirmationPath.StartsWith('/') ? EmailConfirmationPath : "/" + EmailConfirmationPath;
        return $"{baseUrl}{path}?token={Uri.EscapeDataString(token)}";
    }
}
