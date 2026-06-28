using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa.ListarDespesas;

public sealed record ListarDespesasResponseItem(
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
    string? CentroDeCustoNome,
    Guid? FornecedorId,
    bool DespesaFixa);

public sealed record ListarDespesasResponse(
    IReadOnlyList<ListarDespesasResponseItem> Items,
    long Total,
    int Skip,
    int Take);
