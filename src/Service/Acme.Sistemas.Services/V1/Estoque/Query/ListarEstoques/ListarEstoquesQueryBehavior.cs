using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Estoque.Query.ListarEstoques;

public sealed class ListarEstoquesQueryBehavior
    : IPipelineBehavior<ListarEstoquesQuery, ResponseDefault<ListarEstoquesQueryResult>>
{
    public Task<ResponseDefault<ListarEstoquesQueryResult>> Handle(
        ListarEstoquesQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarEstoquesQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
