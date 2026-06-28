namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auth.LoginMobile;

public sealed record LoginMobileResponse(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiraEm,
    DateTime RefreshTokenExpiraEm,
    Guid UsuarioId,
    Guid TenantId,
    string NomeCompleto,
    IReadOnlyList<string> Permissoes);
