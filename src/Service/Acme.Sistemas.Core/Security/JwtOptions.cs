namespace Acme.Sistemas.Core.Security;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SigningKey { get; set; } = string.Empty;
    public int AccessTokenMinutes { get; set; } = 15;
    public int RefreshTokenDays { get; set; } = 7;
    /// <summary>Refresh token de longa duração para clientes mobile (default 90 dias).</summary>
    public int RefreshTokenDaysMobile { get; set; } = 90;
}
