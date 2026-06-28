using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Notificacoes.Query.ListarNotificacoes;

public sealed class ListarNotificacoesQueryBehavior
    : IPipelineBehavior<ListarNotificacoesQuery, ResponseDefault<ListarNotificacoesQueryResult>>
{
    public Task<ResponseDefault<ListarNotificacoesQueryResult>> Handle(
        ListarNotificacoesQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarNotificacoesQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
