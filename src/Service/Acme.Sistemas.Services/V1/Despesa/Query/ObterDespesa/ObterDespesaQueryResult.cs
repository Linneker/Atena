using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Despesa.Query.ObterDespesa;

public sealed record ObterDespesaQueryResult(
    Guid Id,
    string Nome,
    string? Descricao,
    string? Categoria,
    decimal Valor,
    bool DespesaFixa,
    DateTime DataVencimento,
    Guid? CompetenciaId,
    Guid? CentroDeCustoId,
    string? CentroDeCustoNome,
    Guid? FornecedorId,
    StatusPagamento StatusPagamento,
    decimal? ValorPago,
    DateTime? DataPagamento,
    FormaPagamento? FormaPagamento,
    string? ObservacaoPagamento,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
