using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.CentroDeCusto.Command.AlterarCentroDeCusto;

/// <summary>
/// Behavior específico do AlterarCentroDeCustoCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class AlterarCentroDeCustoCommandBehavior
    : IPipelineBehavior<AlterarCentroDeCustoCommand, ResponseDefault<AlterarCentroDeCustoCommandResult>>
{
    public Task<ResponseDefault<AlterarCentroDeCustoCommandResult>> Handle(
        AlterarCentroDeCustoCommand request,
        RequestHandlerDelegate<ResponseDefault<AlterarCentroDeCustoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
