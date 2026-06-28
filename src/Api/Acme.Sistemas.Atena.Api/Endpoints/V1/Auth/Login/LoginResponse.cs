namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auth.Login;

public sealed record LoginResponse(
    string AccessToken,
    string RefreshToken,
    DateTime AccessExpiresAt,
    DateTime RefreshExpiresAt,
    Guid UserId,
    Guid TenantId,
    string NomeCompleto,
    IReadOnlyList<string> Permissions);
