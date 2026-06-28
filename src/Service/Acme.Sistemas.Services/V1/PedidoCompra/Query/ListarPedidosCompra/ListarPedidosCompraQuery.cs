using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Compras;

namespace Acme.Sistemas.Services.V1.PedidoCompra.Query.ListarPedidosCompra;

public sealed record ListarPedidosCompraQuery(
    StatusPedidoCompra? Status = null,
    Guid? FornecedorId = null,
    int Skip = 0,
    int Take = 50) : IRequest<ResponseDefault<ListarPedidosCompraQueryResult>>;
