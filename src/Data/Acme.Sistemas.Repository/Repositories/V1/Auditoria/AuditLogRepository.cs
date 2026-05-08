using System.Data;
using System.Text;
using Acme.Sistemas.Domain.Entities.Auditoria;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Auditoria;

public sealed class AuditLogRepository : IAuditLogRepository
{
    private readonly IDataConfiguration _db;
    private readonly ITenantContext _tenantContext;

    private const string Cols = @"id, tenant_id, user_id, entidade_nome, entidade_id, operacao,
        command_tipo, antes_json, depois_json, ocorrido_em,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public AuditLogRepository(IDataConfiguration db, ITenantContext tenantContext)
    {
        _db = db;
        _tenantContext = tenantContext;
    }

    public Task AddAsync(AuditLog log, CancellationToken cancellationToken = default)
        => _db.ExecuteAsync(@"
            INSERT INTO audit_logs
            (id, tenant_id, user_id, entidade_nome, entidade_id, operacao,
             command_tipo, antes_json, depois_json, ocorrido_em, created_at, created_by)
            VALUES (@id, @tenant_id, @uid, @ent, @eid, @op, @ct, @antes, @depois, @ocorrido, @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = log.Id,
                ["@tenant_id"] = log.TenantId == Guid.Empty ? _tenantContext.TenantId : log.TenantId,
                ["@uid"] = log.UserId,
                ["@ent"] = log.EntidadeNome,
                ["@eid"] = log.EntidadeId,
                ["@op"] = (int)log.Operacao,
                ["@ct"] = log.CommandTipo,
                ["@antes"] = log.AntesJson,
                ["@depois"] = log.DepoisJson,
                ["@ocorrido"] = log.OcorridoEm,
                ["@created_at"] = log.CreatedAt,
                ["@created_by"] = log.CreatedBy
            }, cancellationToken);

    public Task AddApiRequestAsync(ApiRequestAudit audit, CancellationToken cancellationToken = default)
        => _db.ExecuteAsync(@"
            INSERT INTO api_request_audit
            (id, tenant_id, user_id, metodo, caminho, query_string, status_code, duracao_ms,
             ip_address, user_agent, correlation_id, ocorrido_em, created_at, created_by)
            VALUES (@id, @tenant_id, @uid, @met, @cam, @qs, @sc, @dur, @ip, @ua, @corr, @ocorrido, @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = audit.Id,
                ["@tenant_id"] = audit.TenantId,
                ["@uid"] = audit.UserId,
                ["@met"] = audit.Metodo,
                ["@cam"] = audit.Caminho,
                ["@qs"] = audit.QueryString,
                ["@sc"] = audit.StatusCode,
                ["@dur"] = audit.DuracaoMs,
                ["@ip"] = audit.IpAddress,
                ["@ua"] = audit.UserAgent,
                ["@corr"] = audit.CorrelationId,
                ["@ocorrido"] = audit.OcorridoEm,
                ["@created_at"] = audit.CreatedAt,
                ["@created_by"] = audit.CreatedBy
            }, cancellationToken);

    public Task<IReadOnlyList<AuditLog>> ListAsync(
        Guid? userId, string? entidade, OperacaoAuditoria? operacao,
        DateTime? inicio, DateTime? fim, int skip, int take,
        CancellationToken cancellationToken = default)
    {
        var (sql, p) = Filtro(userId, entidade, operacao, inicio, fim);
        sql.Append(" ORDER BY ocorrido_em DESC LIMIT @take OFFSET @skip");
        p["@take"] = take; p["@skip"] = skip;
        return _db.QueryAsync(sql.ToString(), Map, p, cancellationToken);
    }

    public Task<long> CountAsync(
        Guid? userId, string? entidade, OperacaoAuditoria? operacao,
        DateTime? inicio, DateTime? fim, CancellationToken cancellationToken = default)
    {
        var (sql, p) = Filtro(userId, entidade, operacao, inicio, fim, count: true);
        return _db.ExecuteScalarAsync<long>(sql.ToString(), p, cancellationToken);
    }

    private (StringBuilder, Dictionary<string, object?>) Filtro(
        Guid? userId, string? entidade, OperacaoAuditoria? operacao,
        DateTime? inicio, DateTime? fim, bool count = false)
    {
        var sql = new StringBuilder(count
            ? "SELECT COUNT(*) FROM audit_logs WHERE tenant_id = @tenantId AND deleted_at IS NULL"
            : $"SELECT {Cols} FROM audit_logs WHERE tenant_id = @tenantId AND deleted_at IS NULL");
        var p = new Dictionary<string, object?> { ["@tenantId"] = _tenantContext.TenantId };
        if (userId.HasValue) { sql.Append(" AND user_id = @uid"); p["@uid"] = userId.Value; }
        if (!string.IsNullOrWhiteSpace(entidade)) { sql.Append(" AND entidade_nome = @ent"); p["@ent"] = entidade; }
        if (operacao.HasValue) { sql.Append(" AND operacao = @op"); p["@op"] = (int)operacao.Value; }
        if (inicio.HasValue) { sql.Append(" AND ocorrido_em >= @ini"); p["@ini"] = inicio.Value; }
        if (fim.HasValue) { sql.Append(" AND ocorrido_em <= @fim"); p["@fim"] = fim.Value; }
        return (sql, p);
    }

    public Task<IReadOnlyList<AuditLog>> ListHistoricoAsync(string entidade, Guid entidadeId, CancellationToken cancellationToken = default)
        => _db.QueryAsync(
            $@"SELECT {Cols} FROM audit_logs
               WHERE tenant_id = @tenantId AND entidade_nome = @ent AND entidade_id = @eid AND deleted_at IS NULL
               ORDER BY ocorrido_em ASC",
            Map,
            new Dictionary<string, object?>
            {
                ["@tenantId"] = _tenantContext.TenantId,
                ["@ent"] = entidade,
                ["@eid"] = entidadeId
            }, cancellationToken);

    private static AuditLog Map(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        UserId = r.GetValueOrDefault<Guid?>("user_id"),
        EntidadeNome = r.GetValueOrDefault<string>("entidade_nome") ?? string.Empty,
        EntidadeId = r.GetValueOrDefault<Guid?>("entidade_id"),
        Operacao = (OperacaoAuditoria)r.GetValueOrDefault<int>("operacao"),
        CommandTipo = r.GetValueOrDefault<string>("command_tipo") ?? string.Empty,
        AntesJson = r.GetValueOrDefault<string>("antes_json"),
        DepoisJson = r.GetValueOrDefault<string>("depois_json"),
        OcorridoEm = r.GetValueOrDefault<DateTime>("ocorrido_em"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
