using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Configuracao.Query.ObterConfiguracaoRep;

public sealed class ObterConfiguracaoRepQueryBehavior
    : IPipelineBehavior<ObterConfiguracaoRepQuery, ResponseDefault<ObterConfiguracaoRepQueryResult>>
{
    public Task<ResponseDefault<ObterConfiguracaoRepQueryResult>> Handle(
        ObterConfiguracaoRepQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterConfiguracaoRepQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
