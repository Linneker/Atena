using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.RecebimentoCompra.Command.RegistrarRecebimento;

/// <summary>
/// Behavior específico do RegistrarRecebimentoCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class RegistrarRecebimentoCommandBehavior
    : IPipelineBehavior<RegistrarRecebimentoCommand, ResponseDefault<RegistrarRecebimentoCommandResult>>
{
    public Task<ResponseDefault<RegistrarRecebimentoCommandResult>> Handle(
        RegistrarRecebimentoCommand request,
        RequestHandlerDelegate<ResponseDefault<RegistrarRecebimentoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
