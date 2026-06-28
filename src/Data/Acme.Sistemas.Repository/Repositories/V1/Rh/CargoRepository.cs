using System.Data;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Rh;

public sealed class CargoRepository : BaseRepository<Cargo>, ICargoRepository
{
    public CargoRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "cargos";
    protected override Func<IDataRecord, Cargo> Map => MapEntity;

    public override Task AddAsync(Cargo c, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO cargos
                (id, tenant_id, codigo, descricao, codigo_cbo, salario_base_sugerido, ativo, created_at, created_by)
            VALUES (@id, @t, @codigo, @descricao, @cbo, @sbs, @ativo, @createdAt, @createdBy)",
            new Dictionary<string, object?>
            {
                ["@id"] = c.Id,
                ["@t"] = TenantContext.TenantId,
                ["@codigo"] = c.Codigo,
                ["@descricao"] = c.Descricao,
                ["@cbo"] = c.CodigoCbo,
                ["@sbs"] = c.SalarioBaseSugerido,
                ["@ativo"] = c.Ativo ? 1 : 0,
                ["@createdAt"] = c.CreatedAt,
                ["@createdBy"] = c.CreatedBy,
            }, cancellationToken);

    public override Task UpdateAsync(Cargo c, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE cargos SET
                codigo = @codigo, descricao = @descricao, codigo_cbo = @cbo,
                salario_base_sugerido = @sbs, ativo = @ativo,
                updated_at = @updatedAt, updated_by = @updatedBy
            WHERE id = @id AND tenant_id = @t AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = c.Id,
                ["@t"] = TenantContext.TenantId,
                ["@codigo"] = c.Codigo,
                ["@descricao"] = c.Descricao,
                ["@cbo"] = c.CodigoCbo,
                ["@sbs"] = c.SalarioBaseSugerido,
                ["@ativo"] = c.Ativo ? 1 : 0,
                ["@updatedAt"] = DateTime.UtcNow,
                ["@updatedBy"] = c.UpdatedBy,
            }, cancellationToken);

    public Task<Cargo?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(@"
            SELECT * FROM cargos
            WHERE tenant_id = @t AND codigo = @codigo AND deleted_at IS NULL
            LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@t"] = TenantContext.TenantId, ["@codigo"] = codigo },
            cancellationToken);

    private static Cargo MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Codigo = r.GetValueOrDefault<string>("codigo"),
        Descricao = r.GetValueOrDefault<string>("descricao") ?? string.Empty,
        CodigoCbo = r.GetValueOrDefault<string>("codigo_cbo"),
        SalarioBaseSugerido = r.GetValueOrDefault<decimal?>("salario_base_sugerido"),
        Ativo = r.GetValueOrDefault<int>("ativo") == 1,
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by"),
    };
}
