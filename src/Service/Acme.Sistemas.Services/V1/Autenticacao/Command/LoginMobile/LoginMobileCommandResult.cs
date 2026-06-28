namespace Acme.Sistemas.Services.V1.Autenticacao.Command.LoginMobile;

public sealed record LoginMobileCommandResult(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiraEm,
    DateTime RefreshTokenExpiraEm,
    Guid UsuarioId,
    Guid TenantId,
    string NomeCompleto,
    IReadOnlyList<string> Permissoes);
