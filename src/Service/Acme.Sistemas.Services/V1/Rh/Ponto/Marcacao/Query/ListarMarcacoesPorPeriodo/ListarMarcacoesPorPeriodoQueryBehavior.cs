using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Query.ListarMarcacoesPorPeriodo;

public sealed class ListarMarcacoesPorPeriodoQueryBehavior
    : IPipelineBehavior<ListarMarcacoesPorPeriodoQuery, ResponseDefault<ListarMarcacoesPorPeriodoQueryResult>>
{
    public Task<ResponseDefault<ListarMarcacoesPorPeriodoQueryResult>> Handle(
        ListarMarcacoesPorPeriodoQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarMarcacoesPorPeriodoQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
