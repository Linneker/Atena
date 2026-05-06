using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Receita.Query.ListarReceitas;

public sealed record ListarReceitasQueryItem(
    Guid Id,
    string Nome,
    string? Categoria,
    decimal Valor,
    DateTime DataPrevistaRecebimento,
    StatusPagamento StatusRecebimento,
    decimal? ValorRecebido,
    DateTime? DataRecebimento,
    Guid? CompetenciaId,
    Guid? CentroDeCustoId,
    Guid? ClienteId,
    Guid? OrigemVendaId,
    bool ReceitaFixa);

public sealed record ListarReceitasQueryResult(
    IReadOnlyList<ListarReceitasQueryItem> Items,
    long Total,
    int Skip,
    int Take);
