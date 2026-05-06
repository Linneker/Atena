using Acme.Sistemas.Domain.Entities.Auditoria;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog log, CancellationToken cancellationToken = default);
    Task AddApiRequestAsync(ApiRequestAudit audit, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AuditLog>> ListAsync(
        Guid? userId, string? entidade, OperacaoAuditoria? operacao,
        DateTime? inicio, DateTime? fim, int skip, int take,
        CancellationToken cancellationToken = default);

    Task<long> CountAsync(
        Guid? userId, string? entidade, OperacaoAuditoria? operacao,
        DateTime? inicio, DateTime? fim,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AuditLog>> ListHistoricoAsync(
        string entidade, Guid entidadeId, CancellationToken cancellationToken = default);
}
