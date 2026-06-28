using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Query.ObterBeneficioCatalogo;

public sealed class ObterBeneficioCatalogoQueryBehavior
    : IPipelineBehavior<ObterBeneficioCatalogoQuery, ResponseDefault<ObterBeneficioCatalogoQueryResult>>
{
    public Task<ResponseDefault<ObterBeneficioCatalogoQueryResult>> Handle(
        ObterBeneficioCatalogoQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterBeneficioCatalogoQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
