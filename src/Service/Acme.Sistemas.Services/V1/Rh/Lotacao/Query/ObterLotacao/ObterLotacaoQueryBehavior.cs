using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Lotacao.Query.ObterLotacao;

public sealed class ObterLotacaoQueryBehavior
    : IPipelineBehavior<ObterLotacaoQuery, ResponseDefault<ObterLotacaoQueryResult>>
{
    public Task<ResponseDefault<ObterLotacaoQueryResult>> Handle(
        ObterLotacaoQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterLotacaoQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
