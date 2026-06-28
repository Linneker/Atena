using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita.ListarReceitas;

public sealed record ListarReceitasResponseItem(
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
    string? CentroDeCustoNome,
    Guid? ClienteId,
    Guid? OrigemVendaId,
    bool ReceitaFixa);

public sealed record ListarReceitasResponse(
    IReadOnlyList<ListarReceitasResponseItem> Items,
    long Total,
    int Skip,
    int Take);
