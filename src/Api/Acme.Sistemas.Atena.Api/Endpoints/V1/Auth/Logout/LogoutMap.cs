using Acme.Sistemas.Services.V1.Autenticacao.Command.Logout;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auth.Logout;

public static class LogoutMap
{
    public static LogoutCommand ToCommand(this LogoutRequest request)
        => new(request.RefreshToken);
}
