using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Vendas;

namespace Acme.Sistemas.Services.V1.PedidoVenda.Query.ListarPedidosVenda;

public sealed record ListarPedidosVendaQuery(
    StatusPedidoVenda? Status = null,
    Guid? ClienteId = null,
    Guid? VendedorId = null,
    DateTime? Inicio = null,
    DateTime? Fim = null,
    int Skip = 0,
    int Take = 50) : IRequest<ResponseDefault<ListarPedidosVendaQueryResult>>;
