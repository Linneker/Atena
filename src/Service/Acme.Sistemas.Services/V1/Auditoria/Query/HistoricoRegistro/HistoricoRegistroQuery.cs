using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Auditoria;

namespace Acme.Sistemas.Services.V1.Auditoria.Query.HistoricoRegistro;

public sealed record HistoricoRegistroQuery(string Entidade, Guid EntidadeId)
    : IRequest<ResponseDefault<HistoricoRegistroQueryResult>>;

public sealed record HistoricoRegistroItem(
    Guid Id, Guid? UserId, OperacaoAuditoria Operacao, string CommandTipo,
    string? AntesJson, string? DepoisJson, DateTime OcorridoEm);

public sealed record HistoricoRegistroQueryResult(
    string Entidade, Guid EntidadeId, IReadOnlyList<HistoricoRegistroItem> Eventos);
