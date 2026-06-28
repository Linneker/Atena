using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Departamento.Query.ObterDepartamento;

public sealed class ObterDepartamentoQueryBehavior
    : IPipelineBehavior<ObterDepartamentoQuery, ResponseDefault<ObterDepartamentoQueryResult>>
{
    public Task<ResponseDefault<ObterDepartamentoQueryResult>> Handle(
        ObterDepartamentoQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterDepartamentoQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
