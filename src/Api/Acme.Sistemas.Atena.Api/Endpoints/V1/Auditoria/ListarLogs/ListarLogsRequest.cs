using Acme.Sistemas.Domain.Entities.Auditoria;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auditoria.ListarLogs;

public sealed record ListarLogsRequest(
    Guid? UserId = null,
    string? Entidade = null,
    OperacaoAuditoria? Operacao = null,
    DateTime? Inicio = null,
    DateTime? Fim = null,
    int Skip = 0,
    int Take = 50);
