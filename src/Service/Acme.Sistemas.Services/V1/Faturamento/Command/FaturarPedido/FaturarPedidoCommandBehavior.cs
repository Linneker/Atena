using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Faturamento.Command.FaturarPedido;

/// <summary>
/// Behavior específico do FaturarPedidoCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class FaturarPedidoCommandBehavior
    : IPipelineBehavior<FaturarPedidoCommand, ResponseDefault<FaturarPedidoCommandResult>>
{
    public Task<ResponseDefault<FaturarPedidoCommandResult>> Handle(
        FaturarPedidoCommand request,
        RequestHandlerDelegate<ResponseDefault<FaturarPedidoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
