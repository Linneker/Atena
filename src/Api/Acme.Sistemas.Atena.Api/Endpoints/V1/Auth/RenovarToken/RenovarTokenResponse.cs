namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auth.RenovarToken;

public sealed record RenovarTokenResponse(
    string AccessToken,
    string RefreshToken,
    DateTime AccessExpiresAt,
    DateTime RefreshExpiresAt);
