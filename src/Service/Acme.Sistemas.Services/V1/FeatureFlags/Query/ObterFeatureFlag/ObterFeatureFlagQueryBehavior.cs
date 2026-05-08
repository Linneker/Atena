using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.FeatureFlags.Query.ObterFeatureFlag;

/// <summary>
/// Behavior específico do ObterFeatureFlagQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ObterFeatureFlagQueryBehavior
    : IPipelineBehavior<ObterFeatureFlagQuery, ResponseDefault<ObterFeatureFlagQueryResult>>
{
    public Task<ResponseDefault<ObterFeatureFlagQueryResult>> Handle(
        ObterFeatureFlagQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterFeatureFlagQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
