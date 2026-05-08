using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Auditoria;

namespace Acme.Sistemas.Services.V1.Auditoria.Query.ExportarLogs;

public sealed record ExportarLogsQuery(
    Guid? UserId = null,
    string? Entidade = null,
    OperacaoAuditoria? Operacao = null,
    DateTime? Inicio = null,
    DateTime? Fim = null) : IRequest<ResponseDefault<ExportarLogsQueryResult>>;
