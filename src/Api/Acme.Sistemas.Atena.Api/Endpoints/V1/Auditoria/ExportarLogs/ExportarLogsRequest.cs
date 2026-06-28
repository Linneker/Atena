using Acme.Sistemas.Domain.Entities.Auditoria;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auditoria.ExportarLogs;

public sealed record ExportarLogsRequest(
    Guid? UserId = null,
    string? Entidade = null,
    OperacaoAuditoria? Operacao = null,
    DateTime? Inicio = null,
    DateTime? Fim = null);
