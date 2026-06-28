using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Tenant.Query.ObterMeuBranding;

public sealed class ObterMeuBrandingQueryBehavior
    : IPipelineBehavior<ObterMeuBrandingQuery, ResponseDefault<ObterMeuBrandingQueryResult>>
{
    public Task<ResponseDefault<ObterMeuBrandingQueryResult>> Handle(
        ObterMeuBrandingQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterMeuBrandingQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
