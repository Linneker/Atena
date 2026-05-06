using System.Data;
using Acme.Sistemas.Domain.Entities.Produtos;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;
using Acme.Sistemas.Repository.Helper;
using Acme.Sistemas.Repository.Repositories;

namespace Acme.Sistemas.Repository.Repositories.V1.Produtos;

public sealed class TipoProdutoRepository : BaseRepository<TipoProduto>, ITipoProdutoRepository
{
    public TipoProdutoRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "tipos_produto";
    protected override Func<IDataRecord, TipoProduto> Map => MapEntity;

    public override Task AddAsync(TipoProduto t, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO tipos_produto (id, tenant_id, nome, descricao, ativo, created_at, created_by)
            VALUES (@id, @tenant_id, @nome, @descricao, @ativo, @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = t.Id,
                ["@tenant_id"] = TenantContext.TenantId,
                ["@nome"] = t.Nome,
                ["@descricao"] = t.Descricao,
                ["@ativo"] = t.Ativo ? 1 : 0,
                ["@created_at"] = t.CreatedAt,
                ["@created_by"] = t.CreatedBy
            }, cancellationToken);

    public override Task UpdateAsync(TipoProduto t, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE tipos_produto SET nome = @nome, descricao = @descricao, ativo = @ativo,
                updated_at = @updated_at, updated_by = @updated_by
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = t.Id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@nome"] = t.Nome,
                ["@descricao"] = t.Descricao,
                ["@ativo"] = t.Ativo ? 1 : 0,
                ["@updated_at"] = DateTime.UtcNow,
                ["@updated_by"] = t.UpdatedBy
            }, cancellationToken);

    private static TipoProduto MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Nome = r.GetValueOrDefault<string>("nome") ?? string.Empty,
        Descricao = r.GetValueOrDefault<string>("descricao"),
        Ativo = r.GetValueOrDefault<int>("ativo") == 1,
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
