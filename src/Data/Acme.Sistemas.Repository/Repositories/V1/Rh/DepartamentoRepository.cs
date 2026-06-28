using System.Data;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Rh;

public sealed class DepartamentoRepository : BaseRepository<Departamento>, IDepartamentoRepository
{
    public DepartamentoRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "departamentos";
    protected override Func<IDataRecord, Departamento> Map => MapEntity;

    public override Task AddAsync(Departamento d, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO departamentos
                (id, tenant_id, codigo, nome, centro_de_custo_id, ativo, created_at, created_by)
            VALUES (@id, @t, @codigo, @nome, @cc, @ativo, @createdAt, @createdBy)",
            new Dictionary<string, object?>
            {
                ["@id"] = d.Id,
                ["@t"] = TenantContext.TenantId,
                ["@codigo"] = d.Codigo,
                ["@nome"] = d.Nome,
                ["@cc"] = d.CentroDeCustoId,
                ["@ativo"] = d.Ativo ? 1 : 0,
                ["@createdAt"] = d.CreatedAt,
                ["@createdBy"] = d.CreatedBy,
            }, cancellationToken);

    public override Task UpdateAsync(Departamento d, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE departamentos SET
                codigo = @codigo, nome = @nome, centro_de_custo_id = @cc, ativo = @ativo,
                updated_at = @updatedAt, updated_by = @updatedBy
            WHERE id = @id AND tenant_id = @t AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = d.Id,
                ["@t"] = TenantContext.TenantId,
                ["@codigo"] = d.Codigo,
                ["@nome"] = d.Nome,
                ["@cc"] = d.CentroDeCustoId,
                ["@ativo"] = d.Ativo ? 1 : 0,
                ["@updatedAt"] = DateTime.UtcNow,
                ["@updatedBy"] = d.UpdatedBy,
            }, cancellationToken);

    public Task<Departamento?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(@"
            SELECT * FROM departamentos
            WHERE tenant_id = @t AND codigo = @codigo AND deleted_at IS NULL
            LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@t"] = TenantContext.TenantId, ["@codigo"] = codigo },
            cancellationToken);

    private static Departamento MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Codigo = r.GetValueOrDefault<string>("codigo"),
        Nome = r.GetValueOrDefault<string>("nome") ?? string.Empty,
        CentroDeCustoId = r.GetValueOrDefault<Guid?>("centro_de_custo_id"),
        Ativo = r.GetValueOrDefault<int>("ativo") == 1,
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by"),
    };
}
