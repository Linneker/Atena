using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.CentroDeCusto.Command.ExcluirCentroDeCusto;

/// <summary>
/// Behavior específico do ExcluirCentroDeCustoCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ExcluirCentroDeCustoCommandBehavior
    : IPipelineBehavior<ExcluirCentroDeCustoCommand, ResponseDefault>
{
    public Task<ResponseDefault> Handle(
        ExcluirCentroDeCustoCommand request,
        RequestHandlerDelegate<ResponseDefault> next,
        CancellationToken cancellationToken) => next();
}
