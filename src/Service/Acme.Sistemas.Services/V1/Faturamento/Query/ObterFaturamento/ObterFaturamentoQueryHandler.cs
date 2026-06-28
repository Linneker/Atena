using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Faturamento.Query.ObterFaturamento;

public sealed class ObterFaturamentoQueryHandler
    : IRequestHandler<ObterFaturamentoQuery, ResponseDefault<ObterFaturamentoQueryResult>>
{
    private readonly IFaturamentoRepository _repo;
    private readonly IProdutoRepository _produtos;

    public ObterFaturamentoQueryHandler(IFaturamentoRepository repo, IProdutoRepository produtos)
    {
        _repo = repo;
        _produtos = produtos;
    }

    public async Task<ResponseDefault<ObterFaturamentoQueryResult>> Handle(
        ObterFaturamentoQuery request,
        CancellationToken cancellationToken)
    {
        var fat = await _repo.GetByIdAsync(request.Id, cancellationToken);
        if (fat is null)
            return ResponseDefault<ObterFaturamentoQueryResult>.NotFound("Faturamento não encontrado.");

        var itens = await _repo.ListItensAsync(fat.Id, cancellationToken);
        var produtoIds = itens.Select(i => i.ProdutoId);
        var nomes = await _produtos.GetNomesByIdsAsync(produtoIds, cancellationToken);

        var itensResult = itens.Select(i => new ObterFaturamentoItem(
            i.Id, i.PedidoVendaItemId, i.ProdutoId,
            nomes.TryGetValue(i.ProdutoId, out var pn) ? pn : null,
            i.Quantidade, i.PrecoUnitario,
            i.Quantidade * i.PrecoUnitario)).ToList();

        return ResponseDefault<ObterFaturamentoQueryResult>.Ok(new ObterFaturamentoQueryResult(
            fat.Id, fat.Numero, fat.PedidoVendaId,
            fat.DataFaturamento, fat.Tipo, fat.ValorTotal,
            fat.NFeId, fat.ContaReceberId, fat.Observacao,
            itensResult));
    }
}
