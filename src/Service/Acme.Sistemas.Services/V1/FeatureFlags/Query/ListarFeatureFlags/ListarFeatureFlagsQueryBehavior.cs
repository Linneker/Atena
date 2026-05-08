using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.FeatureFlags.Query.ListarFeatureFlags;

/// <summary>
/// Behavior específico do ListarFeatureFlagsQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ListarFeatureFlagsQueryBehavior
    : IPipelineBehavior<ListarFeatureFlagsQuery, ResponseDefault<ListarFeatureFlagsQueryResult>>
{
    public Task<ResponseDefault<ListarFeatureFlagsQueryResult>> Handle(
        ListarFeatureFlagsQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarFeatureFlagsQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
