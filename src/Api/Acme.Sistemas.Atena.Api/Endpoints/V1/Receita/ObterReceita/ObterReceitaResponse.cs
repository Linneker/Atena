using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita.ObterReceita;

public sealed record ObterReceitaResponse(
    Guid Id,
    string Nome,
    string? Descricao,
    string? Categoria,
    decimal Valor,
    bool ReceitaFixa,
    DateTime DataPrevistaRecebimento,
    Guid? CompetenciaId,
    Guid? CentroDeCustoId,
    string? CentroDeCustoNome,
    Guid? ClienteId,
    Guid? OrigemVendaId,
    StatusPagamento StatusRecebimento,
    decimal? ValorRecebido,
    DateTime? DataRecebimento,
    FormaPagamento? FormaPagamento,
    string? ObservacaoRecebimento,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
