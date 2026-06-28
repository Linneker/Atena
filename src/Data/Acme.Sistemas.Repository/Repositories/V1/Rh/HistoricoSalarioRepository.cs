using System.Data;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Rh;

public sealed class HistoricoSalarioRepository : BaseRepository<HistoricoSalario>, IHistoricoSalarioRepository
{
    public HistoricoSalarioRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "historico_salarios";
    protected override Func<IDataRecord, HistoricoSalario> Map => MapEntity;

    public override Task AddAsync(HistoricoSalario h, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO historico_salarios
                (id, tenant_id, funcionario_id, valor, vigencia_inicio, vigencia_fim,
                 motivo, observacao, registrado_por_usuario_id, registrado_at,
                 created_at, created_by)
            VALUES (@id, @t, @fid, @valor, @vi, @vf, @motivo, @obs, @rpid, @rat, @createdAt, @createdBy)",
            new Dictionary<string, object?>
            {
                ["@id"] = h.Id,
                ["@t"] = TenantContext.TenantId,
                ["@fid"] = h.FuncionarioId,
                ["@valor"] = h.Valor,
                ["@vi"] = h.VigenciaInicio.ToDateTime(TimeOnly.MinValue),
                ["@vf"] = h.VigenciaFim?.ToDateTime(TimeOnly.MinValue),
                ["@motivo"] = h.Motivo.ToString(),
                ["@obs"] = h.Observacao,
                ["@rpid"] = h.RegistradoPorUsuarioId,
                ["@rat"] = h.RegistradoAt,
                ["@createdAt"] = h.CreatedAt,
                ["@createdBy"] = h.CreatedBy,
            }, cancellationToken);

    public override Task UpdateAsync(HistoricoSalario h, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE historico_salarios SET
                valor = @valor, vigencia_inicio = @vi, vigencia_fim = @vf,
                motivo = @motivo, observacao = @obs,
                updated_at = @updatedAt, updated_by = @updatedBy
            WHERE id = @id AND tenant_id = @t AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = h.Id,
                ["@t"] = TenantContext.TenantId,
                ["@valor"] = h.Valor,
                ["@vi"] = h.VigenciaInicio.ToDateTime(TimeOnly.MinValue),
                ["@vf"] = h.VigenciaFim?.ToDateTime(TimeOnly.MinValue),
                ["@motivo"] = h.Motivo.ToString(),
                ["@obs"] = h.Observacao,
                ["@updatedAt"] = DateTime.UtcNow,
                ["@updatedBy"] = h.UpdatedBy,
            }, cancellationToken);

    public Task<IReadOnlyList<HistoricoSalario>> ListByFuncionarioAsync(
        Guid funcionarioId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(@"
            SELECT * FROM historico_salarios
            WHERE tenant_id = @t AND funcionario_id = @fid AND deleted_at IS NULL
            ORDER BY vigencia_inicio DESC",
            Map,
            new Dictionary<string, object?> { ["@t"] = TenantContext.TenantId, ["@fid"] = funcionarioId },
            cancellationToken);

    public Task<HistoricoSalario?> GetVigenteAsync(
        Guid funcionarioId, DateOnly em, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(@"
            SELECT * FROM historico_salarios
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
            UPDATE historico_salarios SET
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

    private static HistoricoSalario MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        FuncionarioId = r.GetValueOrDefault<Guid>("funcionario_id"),
        Valor = r.GetValueOrDefault<decimal>("valor"),
        VigenciaInicio = DateOnly.FromDateTime(r.GetValueOrDefault<DateTime>("vigencia_inicio")),
        VigenciaFim = r.GetValueOrDefault<DateTime?>("vigencia_fim") is { } vf ? DateOnly.FromDateTime(vf) : null,
        Motivo = Enum.TryParse<MotivoSalario>(r.GetValueOrDefault<string>("motivo"), out var m) ? m : MotivoSalario.Admissao,
        Observacao = r.GetValueOrDefault<string>("observacao"),
        RegistradoPorUsuarioId = r.GetValueOrDefault<Guid?>("registrado_por_usuario_id"),
        RegistradoAt = r.GetValueOrDefault<DateTime?>("registrado_at"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by"),
    };
}
