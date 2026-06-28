using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.PedidoCompra.Query.ListarPedidosCompra;

public sealed class ListarPedidosCompraQueryBehavior
    : IPipelineBehavior<ListarPedidosCompraQuery, ResponseDefault<ListarPedidosCompraQueryResult>>
{
    public Task<ResponseDefault<ListarPedidosCompraQueryResult>> Handle(
        ListarPedidosCompraQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarPedidosCompraQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
