using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Fechamento.Command.FecharCompetencia;

public sealed class FecharCompetenciaCommandBehavior
    : IPipelineBehavior<FecharCompetenciaCommand, ResponseDefault<FecharCompetenciaCommandResult>>
{
    public Task<ResponseDefault<FecharCompetenciaCommandResult>> Handle(
        FecharCompetenciaCommand request,
        RequestHandlerDelegate<ResponseDefault<FecharCompetenciaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
