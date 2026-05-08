using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.CentroDeCusto.Command.CriarCentroDeCusto;

/// <summary>
/// Behavior específico do CriarCentroDeCustoCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class CriarCentroDeCustoCommandBehavior
    : IPipelineBehavior<CriarCentroDeCustoCommand, ResponseDefault<CriarCentroDeCustoCommandResult>>
{
    public Task<ResponseDefault<CriarCentroDeCustoCommandResult>> Handle(
        CriarCentroDeCustoCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarCentroDeCustoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
