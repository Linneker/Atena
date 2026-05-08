using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.FeatureFlags.Command.RecarregarFeatureFlags;

/// <summary>
/// Behavior específico do RecarregarFeatureFlagsCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class RecarregarFeatureFlagsCommandBehavior
    : IPipelineBehavior<RecarregarFeatureFlagsCommand, ResponseDefault<RecarregarFeatureFlagsCommandResult>>
{
    public Task<ResponseDefault<RecarregarFeatureFlagsCommandResult>> Handle(
        RecarregarFeatureFlagsCommand request,
        RequestHandlerDelegate<ResponseDefault<RecarregarFeatureFlagsCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
