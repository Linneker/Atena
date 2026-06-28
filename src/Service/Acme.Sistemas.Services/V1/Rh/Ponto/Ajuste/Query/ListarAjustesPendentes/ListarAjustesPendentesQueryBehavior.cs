using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Ajuste.Query.ListarAjustesPendentes;

public sealed class ListarAjustesPendentesQueryBehavior
    : IPipelineBehavior<ListarAjustesPendentesQuery, ResponseDefault<ListarAjustesPendentesQueryResult>>
{
    public Task<ResponseDefault<ListarAjustesPendentesQueryResult>> Handle(
        ListarAjustesPendentesQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarAjustesPendentesQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
