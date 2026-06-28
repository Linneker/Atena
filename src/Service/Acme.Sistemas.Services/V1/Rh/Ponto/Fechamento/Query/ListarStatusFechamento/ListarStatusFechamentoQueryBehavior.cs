using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Fechamento.Query.ListarStatusFechamento;

public sealed class ListarStatusFechamentoQueryBehavior
    : IPipelineBehavior<ListarStatusFechamentoQuery, ResponseDefault<ListarStatusFechamentoQueryResult>>
{
    public Task<ResponseDefault<ListarStatusFechamentoQueryResult>> Handle(
        ListarStatusFechamentoQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarStatusFechamentoQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
