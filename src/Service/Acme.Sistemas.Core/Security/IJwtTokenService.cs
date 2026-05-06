namespace Acme.Sistemas.Core.Security;

public interface IJwtTokenService
{
    JwtTokenPair Issue(Guid tenantId, Guid userId, string email, IReadOnlyCollection<string> permissions);
    bool TryValidate(string accessToken, out Guid jti, out Guid tenantId, out Guid userId);
}

public sealed record JwtTokenPair(
    string AccessToken,
    string RefreshToken,
    Guid AccessJti,
    Guid RefreshJti,
    DateTime AccessExpiresAt,
    DateTime RefreshExpiresAt);
