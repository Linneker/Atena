using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.RecebimentoCompra.Command.VincularNFe;

/// <summary>
/// Behavior específico do VincularNFeCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class VincularNFeCommandBehavior
    : IPipelineBehavior<VincularNFeCommand, ResponseDefault<VincularNFeCommandResult>>
{
    public Task<ResponseDefault<VincularNFeCommandResult>> Handle(
        VincularNFeCommand request,
        RequestHandlerDelegate<ResponseDefault<VincularNFeCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
