using System.Data;
using System.Text;
using Acme.Sistemas.Domain.Entities.Compras;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;
using Acme.Sistemas.Repository.Repositories;

namespace Acme.Sistemas.Repository.Repositories.V1.Compras;

public sealed class PedidoCompraRepository : BaseRepository<PedidoCompra>, IPedidoCompraRepository
{
    public PedidoCompraRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "pedidos_compra";
    protected override Func<IDataRecord, PedidoCompra> Map => MapEntity;

    private const string Cols = @"id, tenant_id, numero, fornecedor_id, solicitacao_compra_id,
        data_emissao, previsao_entrega, condicao_pagamento, valor_total, status, observacao,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    private const string ItemCols = @"id, tenant_id, pedido_compra_id, produto_id,
        quantidade, quantidade_recebida, preco_unitario,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(PedidoCompra p, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO pedidos_compra
            (id, tenant_id, numero, fornecedor_id, solicitacao_compra_id,
             data_emissao, previsao_entrega, condicao_pagamento, valor_total, status, observacao,
             created_at, created_by)
            VALUES
            (@id, @tenant_id, @numero, @forn, @sol,
             @emissao, @prev, @cond, @valor, @status, @obs,
             @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = p.Id,
                ["@tenant_id"] = TenantContext.TenantId,
                ["@numero"] = p.Numero,
                ["@forn"] = p.FornecedorId,
                ["@sol"] = p.SolicitacaoCompraId,
                ["@emissao"] = p.DataEmissao,
                ["@prev"] = p.PrevisaoEntrega,
                ["@cond"] = p.CondicaoPagamento,
                ["@valor"] = p.ValorTotal,
                ["@status"] = (int)p.Status,
                ["@obs"] = p.Observacao,
                ["@created_at"] = p.CreatedAt,
                ["@created_by"] = p.CreatedBy
            }, cancellationToken);

    public override Task UpdateAsync(PedidoCompra p, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE pedidos_compra SET
                fornecedor_id = @forn, previsao_entrega = @prev,
                condicao_pagamento = @cond, valor_total = @valor, status = @status, observacao = @obs,
                updated_at = @updated_at, updated_by = @updated_by
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = p.Id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@forn"] = p.FornecedorId,
                ["@prev"] = p.PrevisaoEntrega,
                ["@cond"] = p.CondicaoPagamento,
                ["@valor"] = p.ValorTotal,
                ["@status"] = (int)p.Status,
                ["@obs"] = p.Observacao,
                ["@updated_at"] = DateTime.UtcNow,
                ["@updated_by"] = p.UpdatedBy
            }, cancellationToken);

    public Task UpdateStatusAsync(Guid id, StatusPedidoCompra status, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE pedidos_compra SET status = @s, updated_at = @updated_at
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@s"] = (int)status,
                ["@updated_at"] = DateTime.UtcNow
            }, cancellationToken);

    public Task<PedidoCompra?> GetByNumeroAsync(string numero, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(
            $"SELECT {Cols} FROM pedidos_compra WHERE tenant_id = @tenantId AND numero = @num AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@num"] = numero },
            cancellationToken);

    public Task<IReadOnlyList<PedidoCompra>> ListByFiltroAsync(StatusPedidoCompra? status, Guid? fornecedorId, int skip, int take, CancellationToken cancellationToken = default)
    {
        var (sql, p) = BuildFiltro(status, fornecedorId);
        sql.Append(" ORDER BY data_emissao DESC LIMIT @take OFFSET @skip");
        p["@take"] = take;
        p["@skip"] = skip;
        return Db.QueryAsync(sql.ToString(), Map, p, cancellationToken);
    }

    public Task<long> CountByFiltroAsync(StatusPedidoCompra? status, Guid? fornecedorId, CancellationToken cancellationToken = default)
    {
        var (sql, p) = BuildFiltro(status, fornecedorId, countOnly: true);
        return Db.ExecuteScalarAsync<long>(sql.ToString(), p, cancellationToken);
    }

    private (StringBuilder, Dictionary<string, object?>) BuildFiltro(StatusPedidoCompra? status, Guid? fornecedorId, bool countOnly = false)
    {
        var sql = new StringBuilder(countOnly
            ? "SELECT COUNT(*) FROM pedidos_compra WHERE tenant_id = @tenantId AND deleted_at IS NULL"
            : $"SELECT {Cols} FROM pedidos_compra WHERE tenant_id = @tenantId AND deleted_at IS NULL");
        var p = new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId };
        if (status.HasValue) { sql.Append(" AND status = @s"); p["@s"] = (int)status.Value; }
        if (fornecedorId.HasValue) { sql.Append(" AND fornecedor_id = @f"); p["@f"] = fornecedorId.Value; }
        return (sql, p);
    }

    public Task<IReadOnlyList<PedidoCompraItem>> ListItensAsync(Guid pedidoId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(
            $"SELECT {ItemCols} FROM pedido_compra_itens WHERE tenant_id = @tenantId AND pedido_compra_id = @pid AND deleted_at IS NULL",
            MapItem,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@pid"] = pedidoId },
            cancellationToken);

    public async Task AddItensAsync(IEnumerable<PedidoCompraItem> itens, CancellationToken cancellationToken = default)
    {
        foreach (var i in itens)
        {
            await Db.ExecuteAsync(@"
                INSERT INTO pedido_compra_itens
                (id, tenant_id, pedido_compra_id, produto_id, quantidade, quantidade_recebida, preco_unitario,
                 created_at, created_by)
                VALUES (@id, @tenant_id, @pid, @prod, @qtd, @qrec, @preco, @created_at, @created_by)",
                new Dictionary<string, object?>
                {
                    ["@id"] = i.Id,
                    ["@tenant_id"] = TenantContext.TenantId,
                    ["@pid"] = i.PedidoCompraId,
                    ["@prod"] = i.ProdutoId,
                    ["@qtd"] = i.Quantidade,
                    ["@qrec"] = i.QuantidadeRecebida,
                    ["@preco"] = i.PrecoUnitario,
                    ["@created_at"] = i.CreatedAt,
                    ["@created_by"] = i.CreatedBy
                }, cancellationToken);
        }
    }

    public async Task<int> NextNumeroAsync(CancellationToken cancellationToken = default)
    {
        var count = await Db.ExecuteScalarAsync<long>(
            "SELECT COUNT(*) FROM pedidos_compra WHERE tenant_id = @tenantId",
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId },
            cancellationToken);
        return (int)count + 1;
    }

    private static PedidoCompra MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Numero = r.GetValueOrDefault<string>("numero") ?? string.Empty,
        FornecedorId = r.GetValueOrDefault<Guid>("fornecedor_id"),
        SolicitacaoCompraId = r.GetValueOrDefault<Guid?>("solicitacao_compra_id"),
        DataEmissao = r.GetValueOrDefault<DateTime>("data_emissao"),
        PrevisaoEntrega = r.GetValueOrDefault<DateTime?>("previsao_entrega"),
        CondicaoPagamento = r.GetValueOrDefault<string>("condicao_pagamento"),
        ValorTotal = r.GetValueOrDefault<decimal>("valor_total"),
        Status = (StatusPedidoCompra)r.GetValueOrDefault<int>("status"),
        Observacao = r.GetValueOrDefault<string>("observacao"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };

    private static PedidoCompraItem MapItem(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        PedidoCompraId = r.GetValueOrDefault<Guid>("pedido_compra_id"),
        ProdutoId = r.GetValueOrDefault<Guid>("produto_id"),
        Quantidade = r.GetValueOrDefault<decimal>("quantidade"),
        QuantidadeRecebida = r.GetValueOrDefault<decimal>("quantidade_recebida"),
        PrecoUnitario = r.GetValueOrDefault<decimal>("preco_unitario"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
