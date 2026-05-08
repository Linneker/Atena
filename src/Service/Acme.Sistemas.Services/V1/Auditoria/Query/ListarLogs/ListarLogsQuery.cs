using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Auditoria;

namespace Acme.Sistemas.Services.V1.Auditoria.Query.ListarLogs;

public sealed record ListarLogsQuery(
    Guid? UserId = null,
    string? Entidade = null,
    OperacaoAuditoria? Operacao = null,
    DateTime? Inicio = null,
    DateTime? Fim = null,
    int Skip = 0,
    int Take = 50) : IRequest<ResponseDefault<ListarLogsQueryResult>>;
