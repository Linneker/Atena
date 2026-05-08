using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Auditoria.Query.HistoricoRegistro;

public sealed class HistoricoRegistroQueryBehavior
    : IPipelineBehavior<HistoricoRegistroQuery, ResponseDefault<HistoricoRegistroQueryResult>>
{
    public Task<ResponseDefault<HistoricoRegistroQueryResult>> Handle(
        HistoricoRegistroQuery request,
        RequestHandlerDelegate<ResponseDefault<HistoricoRegistroQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
