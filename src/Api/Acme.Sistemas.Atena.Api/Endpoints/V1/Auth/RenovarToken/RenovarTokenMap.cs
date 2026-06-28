using Acme.Sistemas.Services.V1.Autenticacao.Command.RenovarToken;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auth.RenovarToken;

public static class RenovarTokenMap
{
    public static RenovarTokenCommand ToCommand(this RenovarTokenRequest request, string? userAgent, string? ip)
        => new(request.RefreshToken, userAgent, ip);

    public static RenovarTokenResponse ToResponse(this RenovarTokenCommandResult result)
        => new(result.AccessToken, result.RefreshToken, result.AccessExpiresAt, result.RefreshExpiresAt);
}
