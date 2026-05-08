using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Dashboard.Query.ObterKpis;

/// <summary>
/// Behavior específico do ObterKpisQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ObterKpisQueryBehavior
    : IPipelineBehavior<ObterKpisQuery, ResponseDefault<ObterKpisQueryResult>>
{
    public Task<ResponseDefault<ObterKpisQueryResult>> Handle(
        ObterKpisQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterKpisQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
