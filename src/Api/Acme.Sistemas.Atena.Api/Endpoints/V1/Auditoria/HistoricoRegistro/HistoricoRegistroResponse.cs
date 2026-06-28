using Acme.Sistemas.Domain.Entities.Auditoria;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auditoria.HistoricoRegistro;

public sealed record HistoricoRegistroResponseEvento(
    Guid Id,
    Guid? UserId,
    OperacaoAuditoria Operacao,
    string CommandTipo,
    string? AntesJson,
    string? DepoisJson,
    DateTime OcorridoEm);

public sealed record HistoricoRegistroResponse(
    string Entidade,
    Guid EntidadeId,
    IReadOnlyList<HistoricoRegistroResponseEvento> Eventos);
