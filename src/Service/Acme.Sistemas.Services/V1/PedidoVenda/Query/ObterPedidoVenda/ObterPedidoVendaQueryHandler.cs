using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.PedidoVenda.Query.ObterPedidoVenda;

public sealed class ObterPedidoVendaQueryHandler
    : IRequestHandler<ObterPedidoVendaQuery, ResponseDefault<ObterPedidoVendaQueryResult>>
{
    private readonly IPedidoVendaRepository _repo;
    private readonly IClienteRepository _clientes;
    private readonly IProdutoRepository _produtos;

    public ObterPedidoVendaQueryHandler(
        IPedidoVendaRepository repo,
        IClienteRepository clientes,
        IProdutoRepository produtos)
    {
        _repo = repo;
        _clientes = clientes;
        _produtos = produtos;
    }

    public async Task<ResponseDefault<ObterPedidoVendaQueryResult>> Handle(
        ObterPedidoVendaQuery request,
        CancellationToken cancellationToken)
    {
        var pedido = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (pedido is null)
            return ResponseDefault<ObterPedidoVendaQueryResult>.NotFound("Pedido de venda não encontrado.");

        var itens = await _repo.ListItensAsync(pedido.Id, cancellationToken);

        string? clienteNome = null;
        var nomesCliente = await _clientes.GetNomesByIdsAsync(new[] { pedido.ClienteId }, cancellationToken);
        nomesCliente.TryGetValue(pedido.ClienteId, out clienteNome);

        var produtoIds = itens.Select(i => i.ProdutoId);
        var nomesProduto = await _produtos.GetNomesByIdsAsync(produtoIds, cancellationToken);

        var itensResult = itens.Select(i => new ObterPedidoVendaItem(
            i.Id,
            i.ProdutoId,
            nomesProduto.TryGetValue(i.ProdutoId, out var pn) ? pn : null,
            i.Quantidade,
            i.QuantidadeFaturada,
            i.Quantidade - i.QuantidadeFaturada,
            i.PrecoUnitario,
            i.Quantidade * i.PrecoUnitario)).ToList();

        return ResponseDefault<ObterPedidoVendaQueryResult>.Ok(new ObterPedidoVendaQueryResult(
            pedido.Id, pedido.Numero,
            pedido.ClienteId, clienteNome,
            pedido.VendedorId, pedido.OrcamentoId,
            pedido.DataEmissao, pedido.EstoqueId,
            pedido.ValorTotal, pedido.DescontoPercentual,
            pedido.Status, pedido.CondicaoPagamento, pedido.Observacao,
            itensResult));
    }
}
