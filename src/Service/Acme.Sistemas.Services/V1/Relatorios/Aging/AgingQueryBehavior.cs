using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Relatorios.Aging;

/// <summary>
/// Behavior específico do AgingQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class AgingQueryBehavior
    : IPipelineBehavior<AgingQuery, ResponseDefault<AgingQueryResult>>
{
    public Task<ResponseDefault<AgingQueryResult>> Handle(
        AgingQuery request,
        RequestHandlerDelegate<ResponseDefault<AgingQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
