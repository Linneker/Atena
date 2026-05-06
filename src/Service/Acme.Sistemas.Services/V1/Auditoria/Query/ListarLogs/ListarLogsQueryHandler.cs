using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Auditoria.Query.ListarLogs;

public sealed class ListarLogsQueryHandler
    : IRequestHandler<ListarLogsQuery, ResponseDefault<ListarLogsQueryResult>>
{
    private readonly IAuditLogRepository _repo;

    public ListarLogsQueryHandler(IAuditLogRepository repo) => _repo = repo;

    public async Task<ResponseDefault<ListarLogsQueryResult>> Handle(ListarLogsQuery request, CancellationToken cancellationToken)
    {
        var logs = await _repo.ListAsync(
            request.UserId, request.Entidade, request.Operacao,
            request.Inicio, request.Fim, request.Skip, request.Take, cancellationToken);
        var total = await _repo.CountAsync(
            request.UserId, request.Entidade, request.Operacao,
            request.Inicio, request.Fim, cancellationToken);

        var items = logs.Select(l => new ListarLogsQueryItem(
            l.Id, l.UserId, l.EntidadeNome, l.EntidadeId,
            l.Operacao, l.CommandTipo, l.OcorridoEm)).ToList();

        return ResponseDefault<ListarLogsQueryResult>.Ok(
            new ListarLogsQueryResult(items, total));
    }
}
