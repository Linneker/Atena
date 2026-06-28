using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.PedidoCompra.Query.ObterPedidoCompra;

public sealed class ObterPedidoCompraQueryHandler
    : IRequestHandler<ObterPedidoCompraQuery, ResponseDefault<ObterPedidoCompraQueryResult>>
{
    private readonly IPedidoCompraRepository _repo;
    private readonly IFornecedorRepository _fornecedores;
    private readonly IProdutoRepository _produtos;

    public ObterPedidoCompraQueryHandler(
        IPedidoCompraRepository repo,
        IFornecedorRepository fornecedores,
        IProdutoRepository produtos)
    {
        _repo = repo;
        _fornecedores = fornecedores;
        _produtos = produtos;
    }

    public async Task<ResponseDefault<ObterPedidoCompraQueryResult>> Handle(
        ObterPedidoCompraQuery request,
        CancellationToken cancellationToken)
    {
        var pedido = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (pedido is null)
            return ResponseDefault<ObterPedidoCompraQueryResult>.NotFound("Pedido de compra não encontrado.");

        var itens = await _repo.ListItensAsync(pedido.Id, cancellationToken);

        var nomesForn = await _fornecedores.GetNomesByIdsAsync(new[] { pedido.FornecedorId }, cancellationToken);
        string? fornNome = nomesForn.TryGetValue(pedido.FornecedorId, out var n) ? n : null;

        var produtoIds = itens.Select(i => i.ProdutoId);
        var nomesProd = await _produtos.GetNomesByIdsAsync(produtoIds, cancellationToken);

        var itensResult = itens.Select(i => new ObterPedidoCompraItem(
            i.Id, i.ProdutoId,
            nomesProd.TryGetValue(i.ProdutoId, out var pn) ? pn : null,
            i.Quantidade, i.QuantidadeRecebida,
            i.Quantidade - i.QuantidadeRecebida,
            i.PrecoUnitario, i.Quantidade * i.PrecoUnitario)).ToList();

        return ResponseDefault<ObterPedidoCompraQueryResult>.Ok(new ObterPedidoCompraQueryResult(
            pedido.Id, pedido.Numero,
            pedido.FornecedorId, fornNome,
            pedido.DataEmissao, pedido.PrevisaoEntrega,
            pedido.ValorTotal, pedido.Status,
            pedido.CondicaoPagamento, pedido.Observacao,
            itensResult));
    }
}
