using System.Data;
using Acme.Sistemas.Domain.Entities.Estoque;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;
using Acme.Sistemas.Repository.Repositories;

namespace Acme.Sistemas.Repository.Repositories.V1.Estoque;

public sealed class EstoqueProdutoRepository : BaseRepository<EstoqueProduto>, IEstoqueProdutoRepository
{
    public EstoqueProdutoRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "estoque_produtos";
    protected override Func<IDataRecord, EstoqueProduto> Map => MapEntity;

    private const string Cols = @"id, tenant_id, estoque_id, produto_id, saldo_total, saldo_reservado,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(EstoqueProduto s, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO estoque_produtos
            (id, tenant_id, estoque_id, produto_id, saldo_total, saldo_reservado, created_at, created_by)
            VALUES (@id, @tenant_id, @eid, @pid, @total, @reservado, @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = s.Id,
                ["@tenant_id"] = TenantContext.TenantId,
                ["@eid"] = s.EstoqueId,
                ["@pid"] = s.ProdutoId,
                ["@total"] = s.SaldoTotal,
                ["@reservado"] = s.SaldoReservado,
                ["@created_at"] = s.CreatedAt,
                ["@created_by"] = s.CreatedBy
            }, cancellationToken);

    public override Task UpdateAsync(EstoqueProduto s, CancellationToken cancellationToken = default)
        => UpsertSaldoAsync(s, cancellationToken);

    public Task UpsertSaldoAsync(EstoqueProduto s, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE estoque_produtos SET
                saldo_total = @total, saldo_reservado = @reservado,
                updated_at = @updated_at
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = s.Id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@total"] = s.SaldoTotal,
                ["@reservado"] = s.SaldoReservado,
                ["@updated_at"] = DateTime.UtcNow
            }, cancellationToken);

    public Task AjustarSaldoAsync(Guid estoqueId, Guid produtoId, decimal deltaTotal, decimal deltaReservado, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE estoque_produtos SET
                saldo_total = saldo_total + @dT,
                saldo_reservado = saldo_reservado + @dR,
                updated_at = @updated_at
            WHERE tenant_id = @tenantId AND estoque_id = @eid AND produto_id = @pid AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@tenantId"] = TenantContext.TenantId,
                ["@eid"] = estoqueId,
                ["@pid"] = produtoId,
                ["@dT"] = deltaTotal,
                ["@dR"] = deltaReservado,
                ["@updated_at"] = DateTime.UtcNow
            }, cancellationToken);

    public Task<EstoqueProduto?> GetByEstoqueAndProdutoAsync(Guid estoqueId, Guid produtoId, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(
            $@"SELECT {Cols} FROM estoque_produtos
               WHERE tenant_id = @tenantId AND estoque_id = @eid AND produto_id = @pid AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?>
            {
                ["@tenantId"] = TenantContext.TenantId,
                ["@eid"] = estoqueId,
                ["@pid"] = produtoId
            }, cancellationToken);

    public Task<IReadOnlyList<EstoqueProduto>> ListByProdutoAsync(Guid produtoId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(
            $@"SELECT {Cols} FROM estoque_produtos
               WHERE tenant_id = @tenantId AND produto_id = @pid AND deleted_at IS NULL",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@pid"] = produtoId },
            cancellationToken);

    public Task<IReadOnlyList<EstoqueProduto>> ListByEstoqueAsync(Guid estoqueId, int skip, int take, CancellationToken cancellationToken = default)
        => Db.QueryAsync(
            $@"SELECT {Cols} FROM estoque_produtos
               WHERE tenant_id = @tenantId AND estoque_id = @eid AND deleted_at IS NULL
               ORDER BY produto_id LIMIT @take OFFSET @skip",
            Map,
            new Dictionary<string, object?>
            {
                ["@tenantId"] = TenantContext.TenantId,
                ["@eid"] = estoqueId,
                ["@take"] = take,
                ["@skip"] = skip
            }, cancellationToken);

    private static EstoqueProduto MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        EstoqueId = r.GetValueOrDefault<Guid>("estoque_id"),
        ProdutoId = r.GetValueOrDefault<Guid>("produto_id"),
        SaldoTotal = r.GetValueOrDefault<decimal>("saldo_total"),
        SaldoReservado = r.GetValueOrDefault<decimal>("saldo_reservado"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
