using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Query.ListarPoliticas;

public sealed class ListarPoliticasQueryBehavior
    : IPipelineBehavior<ListarPoliticasQuery, ResponseDefault<ListarPoliticasQueryResult>>
{
    public Task<ResponseDefault<ListarPoliticasQueryResult>> Handle(
        ListarPoliticasQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarPoliticasQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
