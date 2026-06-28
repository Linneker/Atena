using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Ajuste.Command.RejeitarAjuste;

public sealed class RejeitarAjusteCommandBehavior
    : IPipelineBehavior<RejeitarAjusteCommand, ResponseDefault<RejeitarAjusteCommandResult>>
{
    public Task<ResponseDefault<RejeitarAjusteCommandResult>> Handle(
        RejeitarAjusteCommand request,
        RequestHandlerDelegate<ResponseDefault<RejeitarAjusteCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
