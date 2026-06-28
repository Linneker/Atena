using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.ListarStatusFechamento;

public sealed record ListarStatusFechamentoResponseItem(
    Guid FuncionarioId, StatusFechamentoPonto Status, DateTime? FechadoEm);

public sealed record ListarStatusFechamentoResponse(
    IReadOnlyList<ListarStatusFechamentoResponseItem> Items, int Total);
