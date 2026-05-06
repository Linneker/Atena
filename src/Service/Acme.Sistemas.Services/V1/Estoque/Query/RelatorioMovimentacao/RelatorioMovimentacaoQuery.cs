using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Estoque;

namespace Acme.Sistemas.Services.V1.Estoque.Query.RelatorioMovimentacao;

public sealed record RelatorioMovimentacaoQuery(
    Guid ProdutoId,
    DateTime? Inicio = null,
    DateTime? Fim = null,
    int Skip = 0,
    int Take = 200) : IRequest<ResponseDefault<RelatorioMovimentacaoResult>>;

public sealed record MovimentoLinha(
    DateTime Data,
    string Tipo,
    Guid EstoqueId,
    decimal Quantidade,
    decimal? CustoUnitario,
    decimal? CmvUnitario,
    OrigemMovimento Origem,
    string? Motivo,
    string? DocumentoReferencia);

public sealed record RelatorioMovimentacaoResult(
    Guid ProdutoId,
    DateTime? Inicio,
    DateTime? Fim,
    decimal TotalEntradas,
    decimal TotalSaidas,
    decimal Saldo,
    IReadOnlyList<MovimentoLinha> Movimentos);
