using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Cargo.Query.ListarCargos;

public sealed class ListarCargosQueryBehavior
    : IPipelineBehavior<ListarCargosQuery, ResponseDefault<ListarCargosQueryResult>>
{
    public Task<ResponseDefault<ListarCargosQueryResult>> Handle(
        ListarCargosQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarCargosQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
