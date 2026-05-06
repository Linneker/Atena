using System.Data;
using System.Text;
using Acme.Sistemas.Domain.Entities.Vendas;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;
using Acme.Sistemas.Repository.Helper;
using Acme.Sistemas.Repository.Repositories;

namespace Acme.Sistemas.Repository.Repositories.V1.Vendas;

public sealed class FaturamentoRepository : BaseRepository<Faturamento>, IFaturamentoRepository
{
    public FaturamentoRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "faturamentos";
    protected override Func<IDataRecord, Faturamento> Map => MapEntity;

    private const string Cols = @"id, tenant_id, numero, pedido_venda_id, data_faturamento, tipo,
        valor_total, nfe_id, conta_receber_id, observacao,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    private const string ItemCols = @"id, tenant_id, faturamento_id, pedido_venda_item_id, produto_id,
        quantidade, preco_unitario,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(Faturamento f, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO faturamentos
            (id, tenant_id, numero, pedido_venda_id, data_faturamento, tipo,
             valor_total, nfe_id, conta_receber_id, observacao, created_at, created_by)
            VALUES
            (@id, @tenant_id, @num, @ped, @data, @tipo, @valor, @nfe, @cr, @obs, @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = f.Id,
                ["@tenant_id"] = TenantContext.TenantId,
                ["@num"] = f.Numero,
                ["@ped"] = f.PedidoVendaId,
                ["@data"] = f.DataFaturamento,
                ["@tipo"] = (int)f.Tipo,
                ["@valor"] = f.ValorTotal,
                ["@nfe"] = f.NFeId,
                ["@cr"] = f.ContaReceberId,
                ["@obs"] = f.Observacao,
                ["@created_at"] = f.CreatedAt,
                ["@created_by"] = f.CreatedBy
            }, cancellationToken);

    public override Task UpdateAsync(Faturamento entity, CancellationToken cancellationToken = default)
        => throw new InvalidOperationException("Faturamento é imutável.");

    public Task UpdateContaReceberAsync(Guid id, Guid contaReceberId, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE faturamentos SET conta_receber_id = @cr, updated_at = @updated_at
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@cr"] = contaReceberId,
                ["@updated_at"] = DateTime.UtcNow
            }, cancellationToken);

    public Task<IReadOnlyList<Faturamento>> ListByPedidoAsync(Guid pedidoId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(
            $"SELECT {Cols} FROM faturamentos WHERE tenant_id = @tenantId AND pedido_venda_id = @pid AND deleted_at IS NULL ORDER BY data_faturamento DESC",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@pid"] = pedidoId },
            cancellationToken);

    public Task<IReadOnlyList<Faturamento>> ListByFiltroAsync(DateTime? inicio, DateTime? fim, int skip, int take, CancellationToken cancellationToken = default)
    {
        var sql = new StringBuilder($"SELECT {Cols} FROM faturamentos WHERE tenant_id = @tenantId AND deleted_at IS NULL");
        var p = new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId };
        if (inicio.HasValue) { sql.Append(" AND data_faturamento >= @ini"); p["@ini"] = inicio.Value; }
        if (fim.HasValue) { sql.Append(" AND data_faturamento <= @fim"); p["@fim"] = fim.Value; }
        sql.Append(" ORDER BY data_faturamento DESC LIMIT @take OFFSET @skip");
        p["@take"] = take; p["@skip"] = skip;
        return Db.QueryAsync(sql.ToString(), Map, p, cancellationToken);
    }

    public Task<IReadOnlyList<FaturamentoItem>> ListItensAsync(Guid faturamentoId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(
            $"SELECT {ItemCols} FROM faturamento_itens WHERE tenant_id = @tenantId AND faturamento_id = @fid AND deleted_at IS NULL",
            MapItem,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@fid"] = faturamentoId },
            cancellationToken);

    public async Task AddItensAsync(IEnumerable<FaturamentoItem> itens, CancellationToken cancellationToken = default)
    {
        foreach (var i in itens)
        {
            await Db.ExecuteAsync(@"
                INSERT INTO faturamento_itens
                (id, tenant_id, faturamento_id, pedido_venda_item_id, produto_id,
                 quantidade, preco_unitario, created_at, created_by)
                VALUES (@id, @tenant_id, @fid, @pvi, @prod, @qtd, @preco, @created_at, @created_by)",
                new Dictionary<string, object?>
                {
                    ["@id"] = i.Id,
                    ["@tenant_id"] = TenantContext.TenantId,
                    ["@fid"] = i.FaturamentoId,
                    ["@pvi"] = i.PedidoVendaItemId,
                    ["@prod"] = i.ProdutoId,
                    ["@qtd"] = i.Quantidade,
                    ["@preco"] = i.PrecoUnitario,
                    ["@created_at"] = i.CreatedAt,
                    ["@created_by"] = i.CreatedBy
                }, cancellationToken);
        }
    }

    public async Task<int> NextNumeroAsync(CancellationToken cancellationToken = default)
    {
        var c = await Db.ExecuteScalarAsync<long>(
            "SELECT COUNT(*) FROM faturamentos WHERE tenant_id = @tenantId",
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId }, cancellationToken);
        return (int)c + 1;
    }

    private static Faturamento MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Numero = r.GetValueOrDefault<string>("numero") ?? string.Empty,
        PedidoVendaId = r.GetValueOrDefault<Guid>("pedido_venda_id"),
        DataFaturamento = r.GetValueOrDefault<DateTime>("data_faturamento"),
        Tipo = (TipoFaturamento)r.GetValueOrDefault<int>("tipo"),
        ValorTotal = r.GetValueOrDefault<decimal>("valor_total"),
        NFeId = r.GetValueOrDefault<Guid?>("nfe_id"),
        ContaReceberId = r.GetValueOrDefault<Guid?>("conta_receber_id"),
        Observacao = r.GetValueOrDefault<string>("observacao"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };

    private static FaturamentoItem MapItem(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        FaturamentoId = r.GetValueOrDefault<Guid>("faturamento_id"),
        PedidoVendaItemId = r.GetValueOrDefault<Guid>("pedido_venda_item_id"),
        ProdutoId = r.GetValueOrDefault<Guid>("produto_id"),
        Quantidade = r.GetValueOrDefault<decimal>("quantidade"),
        PrecoUnitario = r.GetValueOrDefault<decimal>("preco_unitario"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
