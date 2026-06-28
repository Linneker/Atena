using System.Data;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Rh;

public sealed class FechamentoPontoRepository : BaseRepository<FechamentoPonto>, IFechamentoPontoRepository
{
    public FechamentoPontoRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "fechamentos_ponto";
    protected override Func<IDataRecord, FechamentoPonto> Map => MapEntity;

    public override Task AddAsync(FechamentoPonto f, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO fechamentos_ponto
                (id, tenant_id, funcionario_id, competencia, status, fechado_em, fechado_por,
                 espelho_url, espelho_hash, observacoes, created_at, created_by)
            VALUES (@id, @t, @fid, @comp, @status, @fEm, @fPor, @url, @hash, @obs, @createdAt, @createdBy)",
            new Dictionary<string, object?>
            {
                ["@id"] = f.Id,
                ["@t"] = TenantContext.TenantId,
                ["@fid"] = f.FuncionarioId,
                ["@comp"] = f.Competencia,
                ["@status"] = f.Status.ToString(),
                ["@fEm"] = f.FechadoEm,
                ["@fPor"] = f.FechadoPor,
                ["@url"] = f.EspelhoUrl,
                ["@hash"] = f.EspelhoHash,
                ["@obs"] = f.Observacoes,
                ["@createdAt"] = f.CreatedAt,
                ["@createdBy"] = f.CreatedBy,
            }, cancellationToken);

    public override Task UpdateAsync(FechamentoPonto f, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE fechamentos_ponto SET
                status = @status, fechado_em = @fEm, fechado_por = @fPor,
                reaberto_em = @rEm, reaberto_por = @rPor, motivo_reabertura = @mr,
                espelho_url = @url, espelho_hash = @hash, observacoes = @obs,
                updated_at = @updatedAt, updated_by = @updatedBy
            WHERE id = @id AND tenant_id = @t",
            new Dictionary<string, object?>
            {
                ["@id"] = f.Id,
                ["@t"] = TenantContext.TenantId,
                ["@status"] = f.Status.ToString(),
                ["@fEm"] = f.FechadoEm,
                ["@fPor"] = f.FechadoPor,
                ["@rEm"] = f.ReabertoEm,
                ["@rPor"] = f.ReabertoPor,
                ["@mr"] = f.MotivoReabertura,
                ["@url"] = f.EspelhoUrl,
                ["@hash"] = f.EspelhoHash,
                ["@obs"] = f.Observacoes,
                ["@updatedAt"] = DateTime.UtcNow,
                ["@updatedBy"] = f.UpdatedBy,
            }, cancellationToken);

    public Task<FechamentoPonto?> GetByFuncionarioCompetenciaAsync(
        Guid funcionarioId, string competencia, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(@"
            SELECT * FROM fechamentos_ponto
            WHERE tenant_id = @t AND funcionario_id = @fid AND competencia = @c LIMIT 1",
            Map,
            new Dictionary<string, object?>
            {
                ["@t"] = TenantContext.TenantId,
                ["@fid"] = funcionarioId,
                ["@c"] = competencia,
            }, cancellationToken);

    public Task<IReadOnlyList<FechamentoPonto>> ListByCompetenciaAsync(
        string competencia, CancellationToken cancellationToken = default)
        => Db.QueryAsync(@"
            SELECT * FROM fechamentos_ponto
            WHERE tenant_id = @t AND competencia = @c
            ORDER BY funcionario_id",
            Map,
            new Dictionary<string, object?>
            {
                ["@t"] = TenantContext.TenantId,
                ["@c"] = competencia,
            }, cancellationToken);

    private static FechamentoPonto MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        FuncionarioId = r.GetValueOrDefault<Guid>("funcionario_id"),
        Competencia = r.GetValueOrDefault<string>("competencia") ?? string.Empty,
        Status = Enum.TryParse<StatusFechamentoPonto>(r.GetValueOrDefault<string>("status"), out var s) ? s : StatusFechamentoPonto.Aberto,
        FechadoEm = r.GetValueOrDefault<DateTime?>("fechado_em"),
        FechadoPor = r.GetValueOrDefault<Guid?>("fechado_por"),
        ReabertoEm = r.GetValueOrDefault<DateTime?>("reaberto_em"),
        ReabertoPor = r.GetValueOrDefault<Guid?>("reaberto_por"),
        MotivoReabertura = r.GetValueOrDefault<string>("motivo_reabertura"),
        EspelhoUrl = r.GetValueOrDefault<string>("espelho_url"),
        EspelhoHash = r.GetValueOrDefault<string>("espelho_hash"),
        Observacoes = r.GetValueOrDefault<string>("observacoes"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
    };
}
