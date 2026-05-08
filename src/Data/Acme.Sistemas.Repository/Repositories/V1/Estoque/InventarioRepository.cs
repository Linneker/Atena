using System.Data;
using Acme.Sistemas.Domain.Entities.Estoque;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;
using Acme.Sistemas.Repository.Repositories;

namespace Acme.Sistemas.Repository.Repositories.V1.Estoque;

public sealed class InventarioRepository : BaseRepository<Inventario>, IInventarioRepository
{
    public InventarioRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "inventarios";
    protected override Func<IDataRecord, Inventario> Map => MapEntity;

    private const string ICols = @"id, tenant_id, estoque_id, data_abertura, data_fechamento,
        status, observacao, created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    private const string ItemCols = @"id, tenant_id, inventario_id, produto_id,
        saldo_sistema, saldo_contado, observacao,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(Inventario i, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO inventarios
            (id, tenant_id, estoque_id, data_abertura, data_fechamento, status, observacao,
             created_at, created_by)
            VALUES
            (@id, @tenant_id, @eid, @abertura, @fechamento, @status, @obs,
             @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = i.Id,
                ["@tenant_id"] = TenantContext.TenantId,
                ["@eid"] = i.EstoqueId,
                ["@abertura"] = i.DataAbertura,
                ["@fechamento"] = i.DataFechamento,
                ["@status"] = (int)i.Status,
                ["@obs"] = i.Observacao,
                ["@created_at"] = i.CreatedAt,
                ["@created_by"] = i.CreatedBy
            }, cancellationToken);

    public override Task UpdateAsync(Inventario i, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE inventarios SET
                data_fechamento = @fechamento, status = @status, observacao = @obs,
                updated_at = @updated_at, updated_by = @updated_by
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = i.Id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@fechamento"] = i.DataFechamento,
                ["@status"] = (int)i.Status,
                ["@obs"] = i.Observacao,
                ["@updated_at"] = DateTime.UtcNow,
                ["@updated_by"] = i.UpdatedBy
            }, cancellationToken);

    public Task FecharAsync(Guid inventarioId, DateTime dataFechamento, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE inventarios SET
                status = @status, data_fechamento = @fechamento, updated_at = @updated_at
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = inventarioId,
                ["@tenantId"] = TenantContext.TenantId,
                ["@status"] = (int)StatusInventario.Fechado,
                ["@fechamento"] = dataFechamento,
                ["@updated_at"] = DateTime.UtcNow
            }, cancellationToken);

    public Task<IReadOnlyList<InventarioItem>> ListItensAsync(Guid inventarioId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(
            $@"SELECT {ItemCols} FROM inventario_itens
               WHERE tenant_id = @tenantId AND inventario_id = @id AND deleted_at IS NULL",
            MapItem,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@id"] = inventarioId },
            cancellationToken);

    public async Task AddItensAsync(IEnumerable<InventarioItem> itens, CancellationToken cancellationToken = default)
    {
        foreach (var i in itens)
        {
            await Db.ExecuteAsync(@"
                INSERT INTO inventario_itens
                (id, tenant_id, inventario_id, produto_id, saldo_sistema, saldo_contado,
                 observacao, created_at, created_by)
                VALUES
                (@id, @tenant_id, @inv, @pid, @sis, @cont, @obs, @created_at, @created_by)",
                new Dictionary<string, object?>
                {
                    ["@id"] = i.Id,
                    ["@tenant_id"] = TenantContext.TenantId,
                    ["@inv"] = i.InventarioId,
                    ["@pid"] = i.ProdutoId,
                    ["@sis"] = i.SaldoSistema,
                    ["@cont"] = i.SaldoContado,
                    ["@obs"] = i.Observacao,
                    ["@created_at"] = i.CreatedAt,
                    ["@created_by"] = i.CreatedBy
                }, cancellationToken);
        }
    }

    public Task UpdateItemAsync(InventarioItem i, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE inventario_itens SET
                saldo_contado = @cont, observacao = @obs, updated_at = @updated_at
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = i.Id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@cont"] = i.SaldoContado,
                ["@obs"] = i.Observacao,
                ["@updated_at"] = DateTime.UtcNow
            }, cancellationToken);

    private static Inventario MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        EstoqueId = r.GetValueOrDefault<Guid>("estoque_id"),
        DataAbertura = r.GetValueOrDefault<DateTime>("data_abertura"),
        DataFechamento = r.GetValueOrDefault<DateTime?>("data_fechamento"),
        Status = (StatusInventario)r.GetValueOrDefault<int>("status"),
        Observacao = r.GetValueOrDefault<string>("observacao"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };

    private static InventarioItem MapItem(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        InventarioId = r.GetValueOrDefault<Guid>("inventario_id"),
        ProdutoId = r.GetValueOrDefault<Guid>("produto_id"),
        SaldoSistema = r.GetValueOrDefault<decimal>("saldo_sistema"),
        SaldoContado = r.GetValueOrDefault<decimal?>("saldo_contado"),
        Observacao = r.GetValueOrDefault<string>("observacao"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
