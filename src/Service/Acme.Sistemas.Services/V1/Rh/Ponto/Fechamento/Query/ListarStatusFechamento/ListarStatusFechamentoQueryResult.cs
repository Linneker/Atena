using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Fechamento.Query.ListarStatusFechamento;

public sealed record ListarStatusFechamentoQueryItem(
    Guid FuncionarioId,
    StatusFechamentoPonto Status,
    DateTime? FechadoEm);

public sealed record ListarStatusFechamentoQueryResult(
    IReadOnlyList<ListarStatusFechamentoQueryItem> Items, int Total);
