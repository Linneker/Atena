using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Auditoria.Query.ExportarLogs;

public sealed class ExportarLogsQueryBehavior
    : IPipelineBehavior<ExportarLogsQuery, ResponseDefault<ExportarLogsQueryResult>>
{
    public Task<ResponseDefault<ExportarLogsQueryResult>> Handle(
        ExportarLogsQuery request,
        RequestHandlerDelegate<ResponseDefault<ExportarLogsQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
