using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Cbo.Query.ListarCbos;

public sealed class ListarCbosQueryBehavior
    : IPipelineBehavior<ListarCbosQuery, ResponseDefault<ListarCbosQueryResult>>
{
    public Task<ResponseDefault<ListarCbosQueryResult>> Handle(
        ListarCbosQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarCbosQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
