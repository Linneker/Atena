using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.FluxoDeCaixa.Command.FecharPeriodo;

/// <summary>
/// Behavior específico do FecharPeriodoCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class FecharPeriodoCommandBehavior
    : IPipelineBehavior<FecharPeriodoCommand, ResponseDefault<FecharPeriodoCommandResult>>
{
    public Task<ResponseDefault<FecharPeriodoCommandResult>> Handle(
        FecharPeriodoCommand request,
        RequestHandlerDelegate<ResponseDefault<FecharPeriodoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
