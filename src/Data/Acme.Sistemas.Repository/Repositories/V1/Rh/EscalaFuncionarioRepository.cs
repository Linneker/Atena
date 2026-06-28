using System.Data;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Rh;

public sealed class EscalaFuncionarioRepository : BaseRepository<EscalaFuncionario>, IEscalaFuncionarioRepository
{
    public EscalaFuncionarioRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "escalas_funcionario";
    protected override Func<IDataRecord, EscalaFuncionario> Map => MapEntity;

    public override Task AddAsync(EscalaFuncionario e, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO escalas_funcionario
                (id, tenant_id, funcionario_id, jornada_id, vigencia_inicio, vigencia_fim,
                 observacao, created_at, created_by)
            VALUES (@id, @t, @fid, @jid, @vi, @vf, @obs, @createdAt, @createdBy)",
            new Dictionary<string, object?>
            {
                ["@id"] = e.Id,
                ["@t"] = TenantContext.TenantId,
                ["@fid"] = e.FuncionarioId,
                ["@jid"] = e.JornadaId,
                ["@vi"] = e.VigenciaInicio.ToDateTime(TimeOnly.MinValue),
                ["@vf"] = e.VigenciaFim?.ToDateTime(TimeOnly.MinValue),
                ["@obs"] = e.Observacao,
                ["@createdAt"] = e.CreatedAt,
                ["@createdBy"] = e.CreatedBy,
            }, cancellationToken);

    public override Task UpdateAsync(EscalaFuncionario e, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE escalas_funcionario SET
                jornada_id = @jid, vigencia_inicio = @vi, vigencia_fim = @vf, observacao = @obs,
                updated_at = @updatedAt, updated_by = @updatedBy
            WHERE id = @id AND tenant_id = @t AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = e.Id,
                ["@t"] = TenantContext.TenantId,
                ["@jid"] = e.JornadaId,
                ["@vi"] = e.VigenciaInicio.ToDateTime(TimeOnly.MinValue),
                ["@vf"] = e.VigenciaFim?.ToDateTime(TimeOnly.MinValue),
                ["@obs"] = e.Observacao,
                ["@updatedAt"] = DateTime.UtcNow,
                ["@updatedBy"] = e.UpdatedBy,
            }, cancellationToken);

    public Task<IReadOnlyList<EscalaFuncionario>> ListByFuncionarioAsync(
        Guid funcionarioId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(@"
            SELECT * FROM escalas_funcionario
            WHERE tenant_id = @t AND funcionario_id = @fid AND deleted_at IS NULL
            ORDER BY vigencia_inicio DESC",
            Map,
            new Dictionary<string, object?> { ["@t"] = TenantContext.TenantId, ["@fid"] = funcionarioId },
            cancellationToken);

    public Task<EscalaFuncionario?> GetVigenteAsync(
        Guid funcionarioId, DateOnly em, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(@"
            SELECT * FROM escalas_funcionario
            WHERE tenant_id = @t AND funcionario_id = @fid AND deleted_at IS NULL
              AND vigencia_inicio <= @em
              AND (vigencia_fim IS NULL OR vigencia_fim >= @em)
            ORDER BY vigencia_inicio DESC LIMIT 1",
            Map,
            new Dictionary<string, object?>
            {
                ["@t"] = TenantContext.TenantId,
                ["@fid"] = funcionarioId,
                ["@em"] = em.ToDateTime(TimeOnly.MinValue),
            },
            cancellationToken);

    public Task FecharVigenciaAsync(
        Guid id, DateOnly vigenciaFim, Guid? updatedBy, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE escalas_funcionario SET
                vigencia_fim = @vf, updated_at = @updatedAt, updated_by = @updatedBy
            WHERE id = @id AND tenant_id = @t AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = id,
                ["@t"] = TenantContext.TenantId,
                ["@vf"] = vigenciaFim.ToDateTime(TimeOnly.MinValue),
                ["@updatedAt"] = DateTime.UtcNow,
                ["@updatedBy"] = updatedBy,
            }, cancellationToken);

    private static EscalaFuncionario MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        FuncionarioId = r.GetValueOrDefault<Guid>("funcionario_id"),
        JornadaId = r.GetValueOrDefault<Guid>("jornada_id"),
        VigenciaInicio = DateOnly.FromDateTime(r.GetValueOrDefault<DateTime>("vigencia_inicio")),
        VigenciaFim = r.GetValueOrDefault<DateTime?>("vigencia_fim") is { } vf ? DateOnly.FromDateTime(vf) : null,
        Observacao = r.GetValueOrDefault<string>("observacao"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by"),
    };
}
