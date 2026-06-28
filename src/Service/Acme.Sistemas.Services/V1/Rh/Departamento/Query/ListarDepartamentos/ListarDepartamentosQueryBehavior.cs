using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Departamento.Query.ListarDepartamentos;

public sealed class ListarDepartamentosQueryBehavior
    : IPipelineBehavior<ListarDepartamentosQuery, ResponseDefault<ListarDepartamentosQueryResult>>
{
    public Task<ResponseDefault<ListarDepartamentosQueryResult>> Handle(
        ListarDepartamentosQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarDepartamentosQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
