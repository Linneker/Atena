using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Auditoria.Query.HistoricoRegistro;

public sealed class HistoricoRegistroQueryHandler
    : IRequestHandler<HistoricoRegistroQuery, ResponseDefault<HistoricoRegistroQueryResult>>
{
    private readonly IAuditLogRepository _repo;

    public HistoricoRegistroQueryHandler(IAuditLogRepository repo) => _repo = repo;

    public async Task<ResponseDefault<HistoricoRegistroQueryResult>> Handle(HistoricoRegistroQuery request, CancellationToken cancellationToken)
    {
        var logs = await _repo.ListHistoricoAsync(request.Entidade, request.EntidadeId, cancellationToken);
        var eventos = logs.Select(l => new HistoricoRegistroItem(
            l.Id, l.UserId, l.Operacao, l.CommandTipo,
            l.AntesJson, l.DepoisJson, l.OcorridoEm)).ToList();

        return ResponseDefault<HistoricoRegistroQueryResult>.Ok(
            new HistoricoRegistroQueryResult(request.Entidade, request.EntidadeId, eventos));
    }
}
