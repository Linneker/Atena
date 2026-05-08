using Acme.Sistemas.Domain.Entities.Auditoria;

namespace Acme.Sistemas.Services.V1.Auditoria.Query.ListarLogs;

public sealed record ListarLogsQueryItem(
    Guid Id, Guid? UserId, string Entidade, Guid? EntidadeId,
    OperacaoAuditoria Operacao, string CommandTipo,
    DateTime OcorridoEm);

public sealed record ListarLogsQueryResult(IReadOnlyList<ListarLogsQueryItem> Items, long Total);
