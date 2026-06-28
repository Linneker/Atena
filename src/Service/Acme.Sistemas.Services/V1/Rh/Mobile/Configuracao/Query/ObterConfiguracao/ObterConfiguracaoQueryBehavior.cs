using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Mobile.Configuracao.Query.ObterConfiguracao;

public sealed class ObterConfiguracaoQueryBehavior
    : IPipelineBehavior<ObterConfiguracaoQuery, ResponseDefault<ObterConfiguracaoQueryResult>>
{
    public Task<ResponseDefault<ObterConfiguracaoQueryResult>> Handle(
        ObterConfiguracaoQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterConfiguracaoQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
