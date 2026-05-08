using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.PedidoCompra.Command.CriarPedidoCompra;

/// <summary>
/// Behavior específico do CriarPedidoCompraCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class CriarPedidoCompraCommandBehavior
    : IPipelineBehavior<CriarPedidoCompraCommand, ResponseDefault<CriarPedidoCompraCommandResult>>
{
    public Task<ResponseDefault<CriarPedidoCompraCommandResult>> Handle(
        CriarPedidoCompraCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarPedidoCompraCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
