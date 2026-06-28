using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Mobile.Dispositivos.Query.ListarDispositivos;

public sealed class ListarDispositivosQueryBehavior
    : IPipelineBehavior<ListarDispositivosQuery, ResponseDefault<ListarDispositivosQueryResult>>
{
    public Task<ResponseDefault<ListarDispositivosQueryResult>> Handle(
        ListarDispositivosQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarDispositivosQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
