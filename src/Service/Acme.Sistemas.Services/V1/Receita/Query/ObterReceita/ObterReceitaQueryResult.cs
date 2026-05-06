using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Receita.Query.ObterReceita;

public sealed record ObterReceitaQueryResult(
    Guid Id,
    string Nome,
    string? Descricao,
    string? Categoria,
    decimal Valor,
    bool ReceitaFixa,
    DateTime DataPrevistaRecebimento,
    Guid? CompetenciaId,
    Guid? CentroDeCustoId,
    Guid? ClienteId,
    Guid? OrigemVendaId,
    StatusPagamento StatusRecebimento,
    decimal? ValorRecebido,
    DateTime? DataRecebimento,
    FormaPagamento? FormaPagamento,
    string? ObservacaoRecebimento,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
