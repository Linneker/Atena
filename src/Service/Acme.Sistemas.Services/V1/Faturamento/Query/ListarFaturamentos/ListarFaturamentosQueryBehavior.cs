using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Faturamento.Query.ListarFaturamentos;

public sealed class ListarFaturamentosQueryBehavior
    : IPipelineBehavior<ListarFaturamentosQuery, ResponseDefault<ListarFaturamentosQueryResult>>
{
    public Task<ResponseDefault<ListarFaturamentosQueryResult>> Handle(
        ListarFaturamentosQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarFaturamentosQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
