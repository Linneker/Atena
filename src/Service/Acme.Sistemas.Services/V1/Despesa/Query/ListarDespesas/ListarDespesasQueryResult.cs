using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Despesa.Query.ListarDespesas;

public sealed record ListarDespesasQueryItem(
    Guid Id,
    string Nome,
    string? Categoria,
    decimal Valor,
    DateTime DataVencimento,
    StatusPagamento StatusPagamento,
    decimal? ValorPago,
    DateTime? DataPagamento,
    Guid? CompetenciaId,
    Guid? CentroDeCustoId,
    Guid? FornecedorId,
    bool DespesaFixa);

public sealed record ListarDespesasQueryResult(
    IReadOnlyList<ListarDespesasQueryItem> Items,
    long Total,
    int Skip,
    int Take);
