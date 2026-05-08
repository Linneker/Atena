using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Estoque;

namespace Acme.Sistemas.Services.V1.Estoque.Query.RelatorioMovimentacao;

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
