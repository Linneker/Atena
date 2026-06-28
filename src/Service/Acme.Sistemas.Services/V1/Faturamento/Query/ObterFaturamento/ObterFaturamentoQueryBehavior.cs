using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Faturamento.Query.ObterFaturamento;

public sealed class ObterFaturamentoQueryBehavior
    : IPipelineBehavior<ObterFaturamentoQuery, ResponseDefault<ObterFaturamentoQueryResult>>
{
    public Task<ResponseDefault<ObterFaturamentoQueryResult>> Handle(
        ObterFaturamentoQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterFaturamentoQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
