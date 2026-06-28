using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.PedidoCompra.Query.ObterPedidoCompra;

public sealed class ObterPedidoCompraQueryBehavior
    : IPipelineBehavior<ObterPedidoCompraQuery, ResponseDefault<ObterPedidoCompraQueryResult>>
{
    public Task<ResponseDefault<ObterPedidoCompraQueryResult>> Handle(
        ObterPedidoCompraQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterPedidoCompraQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
