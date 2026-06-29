using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Configuracao.Query.ValidarRep;

public sealed class ValidarRepQueryBehavior
    : IPipelineBehavior<ValidarRepQuery, ResponseDefault<ValidarRepQueryResult>>
{
    public Task<ResponseDefault<ValidarRepQueryResult>> Handle(
        ValidarRepQuery request,
        RequestHandlerDelegate<ResponseDefault<ValidarRepQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
