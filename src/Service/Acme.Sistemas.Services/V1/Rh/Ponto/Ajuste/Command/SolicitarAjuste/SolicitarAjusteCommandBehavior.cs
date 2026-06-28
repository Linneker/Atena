using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Ajuste.Command.SolicitarAjuste;

public sealed class SolicitarAjusteCommandBehavior
    : IPipelineBehavior<SolicitarAjusteCommand, ResponseDefault<SolicitarAjusteCommandResult>>
{
    public Task<ResponseDefault<SolicitarAjusteCommandResult>> Handle(
        SolicitarAjusteCommand request,
        RequestHandlerDelegate<ResponseDefault<SolicitarAjusteCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
