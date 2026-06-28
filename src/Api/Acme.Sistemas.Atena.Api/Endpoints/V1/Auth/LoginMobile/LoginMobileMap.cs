using Acme.Sistemas.Services.V1.Autenticacao.Command.LoginMobile;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auth.LoginMobile;

public static class LoginMobileMap
{
    public static LoginMobileCommand ToCommand(this LoginMobileRequest r, string? ip, string? userAgent)
        => new(r.Email, r.Senha, r.DeviceId, r.Plataforma, ip, userAgent);

    public static LoginMobileResponse ToResponse(this LoginMobileCommandResult r)
        => new(r.AccessToken, r.RefreshToken, r.AccessTokenExpiraEm, r.RefreshTokenExpiraEm,
               r.UsuarioId, r.TenantId, r.NomeCompleto, r.Permissoes);
}
