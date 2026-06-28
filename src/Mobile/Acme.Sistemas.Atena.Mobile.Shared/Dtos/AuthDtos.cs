namespace Acme.Sistemas.Atena.Mobile.Shared.Dtos;

public sealed record LoginMobileRequest(string Email, string Senha, string DeviceId, string Plataforma);

public sealed record LoginMobileResponse(
    string AccessToken,
    string RefreshToken,
    DateTime AccessTokenExpiraEm,
    DateTime RefreshTokenExpiraEm,
    string UsuarioId,
    string FuncionarioId,
    string NomeCompleto,
    IReadOnlyList<string> Permissoes);

public sealed record RefreshTokenRequest(string RefreshToken);

public sealed record RefreshTokenResponse(
    string AccessToken,
    DateTime AccessTokenExpiraEm);
