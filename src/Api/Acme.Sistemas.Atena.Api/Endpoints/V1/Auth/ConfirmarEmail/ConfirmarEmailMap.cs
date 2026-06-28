using Acme.Sistemas.Services.V1.Autenticacao.Command.ConfirmarEmail;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auth.ConfirmarEmail;

public static class ConfirmarEmailMap
{
    public static ConfirmarEmailCommand ToCommand(this ConfirmarEmailRequest request)
        => new(request.Token);

    public static ConfirmarEmailResponse ToResponse(this ConfirmarEmailCommandResult result)
        => new(result.UserId, result.Email, result.ConfirmadoEm);
}
