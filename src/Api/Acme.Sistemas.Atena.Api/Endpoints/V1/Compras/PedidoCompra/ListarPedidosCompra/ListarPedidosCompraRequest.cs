using Acme.Sistemas.Domain.Entities.Compras;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.PedidoCompra.ListarPedidosCompra;

public sealed record ListarPedidosCompraRequest(
    StatusPedidoCompra? Status = null,
    Guid? FornecedorId = null,
    int Skip = 0,
    int Take = 50);
