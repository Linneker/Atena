using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Jornada.Query.ListarJornadas;

public sealed class ListarJornadasQueryBehavior
    : IPipelineBehavior<ListarJornadasQuery, ResponseDefault<ListarJornadasQueryResult>>
{
    public Task<ResponseDefault<ListarJornadasQueryResult>> Handle(
        ListarJornadasQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarJornadasQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
