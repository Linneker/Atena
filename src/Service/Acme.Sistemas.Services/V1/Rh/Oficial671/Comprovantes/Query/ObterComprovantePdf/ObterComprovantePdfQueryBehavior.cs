using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Comprovantes.Query.ObterComprovantePdf;

public sealed class ObterComprovantePdfQueryBehavior
    : IPipelineBehavior<ObterComprovantePdfQuery, ResponseDefault<ObterComprovantePdfQueryResult>>
{
    public Task<ResponseDefault<ObterComprovantePdfQueryResult>> Handle(
        ObterComprovantePdfQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterComprovantePdfQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
