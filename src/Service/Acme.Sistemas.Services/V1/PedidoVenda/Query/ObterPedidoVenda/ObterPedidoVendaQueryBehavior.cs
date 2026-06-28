using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.PedidoVenda.Query.ObterPedidoVenda;

public sealed class ObterPedidoVendaQueryBehavior
    : IPipelineBehavior<ObterPedidoVendaQuery, ResponseDefault<ObterPedidoVendaQueryResult>>
{
    public Task<ResponseDefault<ObterPedidoVendaQueryResult>> Handle(
        ObterPedidoVendaQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterPedidoVendaQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
