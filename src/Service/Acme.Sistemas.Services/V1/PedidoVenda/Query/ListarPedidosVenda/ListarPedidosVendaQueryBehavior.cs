using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.PedidoVenda.Query.ListarPedidosVenda;

public sealed class ListarPedidosVendaQueryBehavior
    : IPipelineBehavior<ListarPedidosVendaQuery, ResponseDefault<ListarPedidosVendaQueryResult>>
{
    public Task<ResponseDefault<ListarPedidosVendaQueryResult>> Handle(
        ListarPedidosVendaQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarPedidosVendaQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
