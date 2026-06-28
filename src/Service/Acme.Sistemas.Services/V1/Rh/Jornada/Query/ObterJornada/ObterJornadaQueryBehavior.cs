using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Jornada.Query.ObterJornada;

public sealed class ObterJornadaQueryBehavior
    : IPipelineBehavior<ObterJornadaQuery, ResponseDefault<ObterJornadaQueryResult>>
{
    public Task<ResponseDefault<ObterJornadaQueryResult>> Handle(
        ObterJornadaQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterJornadaQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
