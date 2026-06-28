using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Cargo.Query.ObterCargo;

public sealed class ObterCargoQueryBehavior
    : IPipelineBehavior<ObterCargoQuery, ResponseDefault<ObterCargoQueryResult>>
{
    public Task<ResponseDefault<ObterCargoQueryResult>> Handle(
        ObterCargoQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterCargoQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
