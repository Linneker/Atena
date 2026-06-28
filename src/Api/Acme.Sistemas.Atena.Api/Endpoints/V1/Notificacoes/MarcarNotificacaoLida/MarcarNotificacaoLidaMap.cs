using Acme.Sistemas.Services.V1.Notificacoes.Command.MarcarNotificacaoLida;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Notificacoes.MarcarNotificacaoLida;

public static class MarcarNotificacaoLidaMap
{
    public static MarcarNotificacaoLidaCommand ToCommand(this MarcarNotificacaoLidaRequest request)
        => new(request.Id);

    public static MarcarNotificacaoLidaResponse ToResponse(this MarcarNotificacaoLidaCommandResult result)
        => new(result.Id, result.Lida);
}
