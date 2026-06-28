using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.PedidoCompra.Query.ListarPedidosCompra;

public sealed class ListarPedidosCompraQueryHandler
    : IRequestHandler<ListarPedidosCompraQuery, ResponseDefault<ListarPedidosCompraQueryResult>>
{
    private readonly IPedidoCompraRepository _repo;
    private readonly IFornecedorRepository _fornecedores;

    public ListarPedidosCompraQueryHandler(IPedidoCompraRepository repo, IFornecedorRepository fornecedores)
    {
        _repo = repo;
        _fornecedores = fornecedores;
    }

    public async Task<ResponseDefault<ListarPedidosCompraQueryResult>> Handle(
        ListarPedidosCompraQuery request,
        CancellationToken cancellationToken)
    {
        var pedidos = await _repo.ListByFiltroAsync(
            request.Status, request.FornecedorId, request.Skip, request.Take, cancellationToken);

        var total = await _repo.CountByFiltroAsync(
            request.Status, request.FornecedorId, cancellationToken);

        var fornecedorIds = pedidos.Select(p => p.FornecedorId);
        var nomesFornecedor = await _fornecedores.GetNomesByIdsAsync(fornecedorIds, cancellationToken);

        var items = pedidos.Select(p => new ListarPedidosCompraQueryItem(
            p.Id, p.Numero,
            p.FornecedorId,
            nomesFornecedor.TryGetValue(p.FornecedorId, out var nome) ? nome : null,
            p.DataEmissao, p.PrevisaoEntrega,
            p.ValorTotal, p.Status)).ToList();

        return ResponseDefault<ListarPedidosCompraQueryResult>.Ok(
            new ListarPedidosCompraQueryResult(items, total, request.Skip, request.Take));
    }
}
