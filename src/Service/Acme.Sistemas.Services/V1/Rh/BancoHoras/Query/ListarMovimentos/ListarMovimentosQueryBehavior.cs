using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Query.ListarMovimentos;

public sealed class ListarMovimentosQueryBehavior
    : IPipelineBehavior<ListarMovimentosQuery, ResponseDefault<ListarMovimentosQueryResult>>
{
    public Task<ResponseDefault<ListarMovimentosQueryResult>> Handle(
        ListarMovimentosQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarMovimentosQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
