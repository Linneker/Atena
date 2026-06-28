using Acme.Sistemas.Domain.Entities.Vendas;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.PedidoVenda.ListarPedidosVenda;

public sealed record ListarPedidosVendaRequest(
    StatusPedidoVenda? Status = null,
    Guid? ClienteId = null,
    Guid? VendedorId = null,
    DateTime? Inicio = null,
    DateTime? Fim = null,
    int Skip = 0,
    int Take = 50);
