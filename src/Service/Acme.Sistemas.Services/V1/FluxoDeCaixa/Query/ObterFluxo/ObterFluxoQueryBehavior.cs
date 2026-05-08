using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.FluxoDeCaixa.Query.ObterFluxo;

/// <summary>
/// Behavior específico do ObterFluxoQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ObterFluxoQueryBehavior
    : IPipelineBehavior<ObterFluxoQuery, ResponseDefault<ObterFluxoQueryResult>>
{
    public Task<ResponseDefault<ObterFluxoQueryResult>> Handle(
        ObterFluxoQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterFluxoQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
