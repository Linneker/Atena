using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita.ListarReceitas;

public sealed record ListarReceitasRequest(
    StatusPagamento? Status = null,
    DateTime? RecebimentoInicio = null,
    DateTime? RecebimentoFim = null,
    string? Categoria = null,
    Guid? CompetenciaId = null,
    int Skip = 0,
    int Take = 50);
