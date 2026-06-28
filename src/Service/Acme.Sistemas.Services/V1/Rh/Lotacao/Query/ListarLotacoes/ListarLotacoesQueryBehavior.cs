using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Lotacao.Query.ListarLotacoes;

public sealed class ListarLotacoesQueryBehavior
    : IPipelineBehavior<ListarLotacoesQuery, ResponseDefault<ListarLotacoesQueryResult>>
{
    public Task<ResponseDefault<ListarLotacoesQueryResult>> Handle(
        ListarLotacoesQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarLotacoesQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
