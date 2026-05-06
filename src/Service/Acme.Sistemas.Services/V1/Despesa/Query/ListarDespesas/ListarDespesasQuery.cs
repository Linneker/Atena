using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Despesa.Query.ListarDespesas;

public sealed record ListarDespesasQuery(
    StatusPagamento? Status = null,
    DateTime? VencimentoInicio = null,
    DateTime? VencimentoFim = null,
    string? Categoria = null,
    Guid? CompetenciaId = null,
    int Skip = 0,
    int Take = 50) : IRequest<ResponseDefault<ListarDespesasQueryResult>>;
