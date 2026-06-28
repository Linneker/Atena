using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa.ListarDespesas;

public sealed record ListarDespesasRequest(
    StatusPagamento? Status = null,
    DateTime? VencimentoInicio = null,
    DateTime? VencimentoFim = null,
    string? Categoria = null,
    Guid? CompetenciaId = null,
    int Skip = 0,
    int Take = 50);
