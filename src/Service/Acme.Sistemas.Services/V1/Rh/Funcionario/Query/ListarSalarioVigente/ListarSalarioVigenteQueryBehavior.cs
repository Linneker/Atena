using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Query.ListarSalarioVigente;

public sealed class ListarSalarioVigenteQueryBehavior
    : IPipelineBehavior<ListarSalarioVigenteQuery, ResponseDefault<ListarSalarioVigenteQueryResult>>
{
    public Task<ResponseDefault<ListarSalarioVigenteQueryResult>> Handle(
        ListarSalarioVigenteQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarSalarioVigenteQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
