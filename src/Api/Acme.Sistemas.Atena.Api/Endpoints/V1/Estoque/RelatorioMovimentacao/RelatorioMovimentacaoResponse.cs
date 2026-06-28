using Acme.Sistemas.Domain.Entities.Estoque;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Estoque.RelatorioMovimentacao;

public sealed record RelatorioMovimentacaoResponseLinha(
    DateTime Data,
    string Tipo,
    Guid EstoqueId,
    decimal Quantidade,
    decimal? CustoUnitario,
    decimal? CmvUnitario,
    OrigemMovimento Origem,
    string? Motivo,
    string? DocumentoReferencia);

public sealed record RelatorioMovimentacaoResponse(
    Guid ProdutoId,
    DateTime? Inicio,
    DateTime? Fim,
    decimal TotalEntradas,
    decimal TotalSaidas,
    decimal Saldo,
    IReadOnlyList<RelatorioMovimentacaoResponseLinha> Movimentos);
