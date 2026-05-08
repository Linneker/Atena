using System.Data;
using Acme.Sistemas.Domain.Entities.Estoque;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;
using Acme.Sistemas.Repository.Repositories;
using EstoqueEntity = Acme.Sistemas.Domain.Entities.Estoque.Estoque;

namespace Acme.Sistemas.Repository.Repositories.V1.Estoque;

public sealed class EstoqueRepository : BaseRepository<EstoqueEntity>, IEstoqueRepository
{
    public EstoqueRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "estoques";
    protected override Func<IDataRecord, EstoqueEntity> Map => MapEntity;

    private const string Cols = @"id, tenant_id, codigo, nome, localizacao,
        permite_saldo_negativo, ativo,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(EstoqueEntity e, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO estoques
            (id, tenant_id, codigo, nome, localizacao, permite_saldo_negativo, ativo,
             created_at, created_by)
            VALUES
            (@id, @tenant_id, @codigo, @nome, @loc, @psn, @ativo, @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = e.Id,
                ["@tenant_id"] = TenantContext.TenantId,
                ["@codigo"] = e.Codigo,
                ["@nome"] = e.Nome,
                ["@loc"] = e.Localizacao,
                ["@psn"] = e.PermiteSaldoNegativo ? 1 : 0,
                ["@ativo"] = e.Ativo ? 1 : 0,
                ["@created_at"] = e.CreatedAt,
                ["@created_by"] = e.CreatedBy
            }, cancellationToken);

    public override Task UpdateAsync(EstoqueEntity e, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE estoques SET nome = @nome, localizacao = @loc,
                permite_saldo_negativo = @psn, ativo = @ativo,
                updated_at = @updated_at, updated_by = @updated_by
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = e.Id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@nome"] = e.Nome,
                ["@loc"] = e.Localizacao,
                ["@psn"] = e.PermiteSaldoNegativo ? 1 : 0,
                ["@ativo"] = e.Ativo ? 1 : 0,
                ["@updated_at"] = DateTime.UtcNow,
                ["@updated_by"] = e.UpdatedBy
            }, cancellationToken);

    public Task<EstoqueEntity?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(
            $"SELECT {Cols} FROM estoques WHERE tenant_id = @tenantId AND codigo = @codigo AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@codigo"] = codigo },
            cancellationToken);

    private static EstoqueEntity MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Codigo = r.GetValueOrDefault<string>("codigo") ?? string.Empty,
        Nome = r.GetValueOrDefault<string>("nome") ?? string.Empty,
        Localizacao = r.GetValueOrDefault<string>("localizacao"),
        PermiteSaldoNegativo = r.GetValueOrDefault<int>("permite_saldo_negativo") == 1,
        Ativo = r.GetValueOrDefault<int>("ativo") == 1,
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
