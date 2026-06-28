using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Fechamento.Command.ReabrirCompetencia;

public sealed class ReabrirCompetenciaCommandBehavior
    : IPipelineBehavior<ReabrirCompetenciaCommand, ResponseDefault<ReabrirCompetenciaCommandResult>>
{
    public Task<ResponseDefault<ReabrirCompetenciaCommandResult>> Handle(
        ReabrirCompetenciaCommand request,
        RequestHandlerDelegate<ResponseDefault<ReabrirCompetenciaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
