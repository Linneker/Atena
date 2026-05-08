using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Auditoria.Query.ListarLogs;

/// <summary>
/// Behavior específico do ListarLogsQuery. Sem regras adicionais — auditoria não cacheia
/// (dados sensíveis e voláteis); o pipeline transversal (Validation → Audit → Log) é suficiente.
/// </summary>
public sealed class ListarLogsQueryBehavior
    : IPipelineBehavior<ListarLogsQuery, ResponseDefault<ListarLogsQueryResult>>
{
    public Task<ResponseDefault<ListarLogsQueryResult>> Handle(
        ListarLogsQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarLogsQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
