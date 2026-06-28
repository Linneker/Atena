using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Notificacoes.Command.MarcarNotificacaoLida;

public sealed class MarcarNotificacaoLidaCommandBehavior
    : IPipelineBehavior<MarcarNotificacaoLidaCommand, ResponseDefault<MarcarNotificacaoLidaCommandResult>>
{
    public Task<ResponseDefault<MarcarNotificacaoLidaCommandResult>> Handle(
        MarcarNotificacaoLidaCommand request,
        RequestHandlerDelegate<ResponseDefault<MarcarNotificacaoLidaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
