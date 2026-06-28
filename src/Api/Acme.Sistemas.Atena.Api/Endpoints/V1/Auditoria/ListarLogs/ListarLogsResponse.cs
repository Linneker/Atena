using Acme.Sistemas.Domain.Entities.Auditoria;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auditoria.ListarLogs;

public sealed record ListarLogsResponseItem(
    Guid Id,
    Guid? UserId,
    string Entidade,
    Guid? EntidadeId,
    OperacaoAuditoria Operacao,
    string CommandTipo,
    DateTime OcorridoEm);

public sealed record ListarLogsResponse(
    IReadOnlyList<ListarLogsResponseItem> Items,
    long Total);
