using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.FeatureFlags.Command.AlterarFeatureFlag;

/// <summary>
/// Behavior específico do AlterarFeatureFlagCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class AlterarFeatureFlagCommandBehavior
    : IPipelineBehavior<AlterarFeatureFlagCommand, ResponseDefault<AlterarFeatureFlagCommandResult>>
{
    public Task<ResponseDefault<AlterarFeatureFlagCommandResult>> Handle(
        AlterarFeatureFlagCommand request,
        RequestHandlerDelegate<ResponseDefault<AlterarFeatureFlagCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
