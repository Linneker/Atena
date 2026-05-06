using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Estoque.Query.RelatorioMovimentacao;

public sealed class RelatorioMovimentacaoQueryHandler
    : IRequestHandler<RelatorioMovimentacaoQuery, ResponseDefault<RelatorioMovimentacaoResult>>
{
    private readonly IEntradaProdutoEstoqueRepository _entradas;
    private readonly ISaidaProdutoEstoqueRepository _saidas;

    public RelatorioMovimentacaoQueryHandler(
        IEntradaProdutoEstoqueRepository entradas,
        ISaidaProdutoEstoqueRepository saidas)
    {
        _entradas = entradas;
        _saidas = saidas;
    }

    public async Task<ResponseDefault<RelatorioMovimentacaoResult>> Handle(RelatorioMovimentacaoQuery request, CancellationToken cancellationToken)
    {
        var entradas = await _entradas.ListByProdutoAsync(
            request.ProdutoId, request.Inicio, request.Fim, request.Skip, request.Take, cancellationToken);
        var saidas = await _saidas.ListByProdutoAsync(
            request.ProdutoId, request.Inicio, request.Fim, request.Skip, request.Take, cancellationToken);

        var linhas = entradas
            .Select(e => new MovimentoLinha(
                e.DataMovimento, "Entrada", e.EstoqueId, e.Quantidade,
                e.CustoUnitario, null, e.Origem, e.Motivo, e.DocumentoReferencia))
            .Concat(saidas.Select(s => new MovimentoLinha(
                s.DataMovimento, "Saida", s.EstoqueId, s.Quantidade,
                s.CustoUnitario, s.CmvUnitario, s.Origem, s.Motivo, s.DocumentoReferencia)))
            .OrderByDescending(m => m.Data)
            .ToList();

        var totalEntradas = entradas.Sum(e => e.Quantidade);
        var totalSaidas = saidas.Sum(s => s.Quantidade);

        return ResponseDefault<RelatorioMovimentacaoResult>.Ok(new RelatorioMovimentacaoResult(
            request.ProdutoId, request.Inicio, request.Fim,
            totalEntradas, totalSaidas, totalEntradas - totalSaidas,
            linhas));
    }
}
