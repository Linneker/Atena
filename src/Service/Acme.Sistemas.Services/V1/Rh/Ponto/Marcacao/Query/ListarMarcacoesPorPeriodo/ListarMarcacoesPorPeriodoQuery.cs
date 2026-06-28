using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Query.ListarMarcacoesPorPeriodo;

public sealed record ListarMarcacoesPorPeriodoQuery(
    Guid FuncionarioId,
    DateOnly DataInicio,
    DateOnly DataFim) : IRequest<ResponseDefault<ListarMarcacoesPorPeriodoQueryResult>>;
