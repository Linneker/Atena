using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Receita.Query.ListarReceitas;

public sealed record ListarReceitasQuery(
    StatusPagamento? Status = null,
    DateTime? RecebimentoInicio = null,
    DateTime? RecebimentoFim = null,
    string? Categoria = null,
    Guid? CompetenciaId = null,
    int Skip = 0,
    int Take = 50) : IRequest<ResponseDefault<ListarReceitasQueryResult>>;
