using System.Data;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Rh;

public sealed class BeneficioFuncionarioRepository : BaseRepository<BeneficioFuncionario>, IBeneficioFuncionarioRepository
{
    public BeneficioFuncionarioRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "beneficios_funcionario";
    protected override Func<IDataRecord, BeneficioFuncionario> Map => MapEntity;

    public override Task AddAsync(BeneficioFuncionario b, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO beneficios_funcionario
                (id, tenant_id, funcionario_id, beneficio_catalogo_id, valor,
                 desconto_funcionario_pct, vigencia_inicio, vigencia_fim, observacao,
                 created_at, created_by)
            VALUES (@id, @t, @fid, @bid, @valor, @dfp, @vi, @vf, @obs, @createdAt, @createdBy)",
            new Dictionary<string, object?>
            {
                ["@id"] = b.Id,
                ["@t"] = TenantContext.TenantId,
                ["@fid"] = b.FuncionarioId,
                ["@bid"] = b.BeneficioCatalogoId,
                ["@valor"] = b.Valor,
                ["@dfp"] = b.DescontoFuncionarioPct,
                ["@vi"] = b.VigenciaInicio.ToDateTime(TimeOnly.MinValue),
                ["@vf"] = b.VigenciaFim?.ToDateTime(TimeOnly.MinValue),
                ["@obs"] = b.Observacao,
                ["@createdAt"] = b.CreatedAt,
                ["@createdBy"] = b.CreatedBy,
            }, cancellationToken);

    public override Task UpdateAsync(BeneficioFuncionario b, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE beneficios_funcionario SET
                valor = @valor, desconto_funcionario_pct = @dfp,
                vigencia_inicio = @vi, vigencia_fim = @vf, observacao = @obs,
                updated_at = @updatedAt, updated_by = @updatedBy
            WHERE id = @id AND tenant_id = @t AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = b.Id,
                ["@t"] = TenantContext.TenantId,
                ["@valor"] = b.Valor,
                ["@dfp"] = b.DescontoFuncionarioPct,
                ["@vi"] = b.VigenciaInicio.ToDateTime(TimeOnly.MinValue),
                ["@vf"] = b.VigenciaFim?.ToDateTime(TimeOnly.MinValue),
                ["@obs"] = b.Observacao,
                ["@updatedAt"] = DateTime.UtcNow,
                ["@updatedBy"] = b.UpdatedBy,
            }, cancellationToken);

    public Task<IReadOnlyList<BeneficioFuncionario>> ListByFuncionarioAsync(
        Guid funcionarioId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(@"
            SELECT * FROM beneficios_funcionario
            WHERE tenant_id = @t AND funcionario_id = @fid AND deleted_at IS NULL
            ORDER BY vigencia_inicio DESC",
            Map,
            new Dictionary<string, object?> { ["@t"] = TenantContext.TenantId, ["@fid"] = funcionarioId },
            cancellationToken);

    public Task<BeneficioFuncionario?> GetVigenteAsync(
        Guid funcionarioId, Guid beneficioCatalogoId, DateOnly em, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(@"
            SELECT * FROM beneficios_funcionario
            WHERE tenant_id = @t AND funcionario_id = @fid AND beneficio_catalogo_id = @bid
              AND deleted_at IS NULL
              AND vigencia_inicio <= @em
              AND (vigencia_fim IS NULL OR vigencia_fim >= @em)
            LIMIT 1",
            Map,
            new Dictionary<string, object?>
            {
                ["@t"] = TenantContext.TenantId,
                ["@fid"] = funcionarioId,
                ["@bid"] = beneficioCatalogoId,
                ["@em"] = em.ToDateTime(TimeOnly.MinValue),
            },
            cancellationToken);

    private static BeneficioFuncionario MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        FuncionarioId = r.GetValueOrDefault<Guid>("funcionario_id"),
        BeneficioCatalogoId = r.GetValueOrDefault<Guid>("beneficio_catalogo_id"),
        Valor = r.GetValueOrDefault<decimal?>("valor"),
        DescontoFuncionarioPct = r.GetValueOrDefault<decimal?>("desconto_funcionario_pct"),
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
