using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Notificacoes.Command.MarcarNotificacaoLida;

// TODO: Stub temporário. Implementação futura: UPDATE notificacoes SET lida=1 WHERE id=@id.
public sealed class MarcarNotificacaoLidaCommandHandler
    : IRequestHandler<MarcarNotificacaoLidaCommand, ResponseDefault<MarcarNotificacaoLidaCommandResult>>
{
    public Task<ResponseDefault<MarcarNotificacaoLidaCommandResult>> Handle(
        MarcarNotificacaoLidaCommand request,
        CancellationToken cancellationToken)
    {
        var result = new MarcarNotificacaoLidaCommandResult(request.Id, Lida: true);
        return Task.FromResult(ResponseDefault<MarcarNotificacaoLidaCommandResult>.Ok(result));
    }
}
