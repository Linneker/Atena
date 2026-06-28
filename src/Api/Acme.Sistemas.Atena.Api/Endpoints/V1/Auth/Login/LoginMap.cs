using Acme.Sistemas.Services.V1.Autenticacao.Command.Login;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auth.Login;

public static class LoginMap
{
    public static LoginCommand ToCommand(this LoginRequest request, string? userAgent, string? ip)
        => new(request.Email, request.Senha, userAgent, ip);

    public static LoginResponse ToResponse(this LoginCommandResult result)
        => new(
            result.AccessToken,
            result.RefreshToken,
            result.AccessExpiresAt,
            result.RefreshExpiresAt,
            result.UserId,
            result.TenantId,
            result.NomeCompleto,
            result.Permissions);
}
