using System.Data;
using System.Text;
using Acme.Sistemas.Domain.Entities.Vendas;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;
using Acme.Sistemas.Repository.Helper;
using Acme.Sistemas.Repository.Repositories;

namespace Acme.Sistemas.Repository.Repositories.V1.Vendas;

public sealed class PedidoVendaRepository : BaseRepository<PedidoVenda>, IPedidoVendaRepository
{
    public PedidoVendaRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "pedidos_venda";
    protected override Func<IDataRecord, PedidoVenda> Map => MapEntity;

    private const string Cols = @"id, tenant_id, numero, cliente_id, vendedor_id, orcamento_id,
        data_emissao, estoque_id, valor_total, desconto_percentual, status,
        condicao_pagamento, observacao,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    private const string ItemCols = @"id, tenant_id, pedido_venda_id, produto_id,
        quantidade, quantidade_faturada, preco_unitario,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(PedidoVenda p, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO pedidos_venda
            (id, tenant_id, numero, cliente_id, vendedor_id, orcamento_id,
             data_emissao, estoque_id, valor_total, desconto_percentual, status,
             condicao_pagamento, observacao, created_at, created_by)
            VALUES
            (@id, @tenant_id, @num, @cli, @vend, @orc, @em, @eid, @valor, @desc, @status,
             @cond, @obs, @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = p.Id,
                ["@tenant_id"] = TenantContext.TenantId,
                ["@num"] = p.Numero,
                ["@cli"] = p.ClienteId,
                ["@vend"] = p.VendedorId,
                ["@orc"] = p.OrcamentoId,
                ["@em"] = p.DataEmissao,
                ["@eid"] = p.EstoqueId,
                ["@valor"] = p.ValorTotal,
                ["@desc"] = p.DescontoPercentual,
                ["@status"] = (int)p.Status,
                ["@cond"] = p.CondicaoPagamento,
                ["@obs"] = p.Observacao,
                ["@created_at"] = p.CreatedAt,
                ["@created_by"] = p.CreatedBy
            }, cancellationToken);

    public override Task UpdateAsync(PedidoVenda p, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE pedidos_venda SET
                cliente_id = @cli, vendedor_id = @vend, valor_total = @valor,
                desconto_percentual = @desc, status = @status,
                condicao_pagamento = @cond, observacao = @obs,
                updated_at = @updated_at, updated_by = @updated_by
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = p.Id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@cli"] = p.ClienteId,
                ["@vend"] = p.VendedorId,
                ["@valor"] = p.ValorTotal,
                ["@desc"] = p.DescontoPercentual,
                ["@status"] = (int)p.Status,
                ["@cond"] = p.CondicaoPagamento,
                ["@obs"] = p.Observacao,
                ["@updated_at"] = DateTime.UtcNow,
                ["@updated_by"] = p.UpdatedBy
            }, cancellationToken);

    public Task UpdateStatusAsync(Guid id, StatusPedidoVenda status, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE pedidos_venda SET status = @s, updated_at = @updated_at
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@s"] = (int)status,
                ["@updated_at"] = DateTime.UtcNow
            }, cancellationToken);

    public Task UpdateItemQuantidadeFaturadaAsync(Guid itemId, decimal nova, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE pedido_venda_itens SET quantidade_faturada = @qf, updated_at = @updated_at
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = itemId,
                ["@tenantId"] = TenantContext.TenantId,
                ["@qf"] = nova,
                ["@updated_at"] = DateTime.UtcNow
            }, cancellationToken);

    public Task<IReadOnlyList<PedidoVenda>> ListByFiltroAsync(StatusPedidoVenda? status, Guid? clienteId, Guid? vendedorId, DateTime? inicio, DateTime? fim, int skip, int take, CancellationToken cancellationToken = default)
    {
        var (sql, p) = Filtro(status, clienteId, vendedorId, inicio, fim);
        sql.Append(" ORDER BY data_emissao DESC LIMIT @take OFFSET @skip");
        p["@take"] = take; p["@skip"] = skip;
        return Db.QueryAsync(sql.ToString(), Map, p, cancellationToken);
    }

    public Task<long> CountByFiltroAsync(StatusPedidoVenda? status, Guid? clienteId, Guid? vendedorId, DateTime? inicio, DateTime? fim, CancellationToken cancellationToken = default)
    {
        var (sql, p) = Filtro(status, clienteId, vendedorId, inicio, fim, count: true);
        return Db.ExecuteScalarAsync<long>(sql.ToString(), p, cancellationToken);
    }

    private (StringBuilder, Dictionary<string, object?>) Filtro(StatusPedidoVenda? status, Guid? clienteId, Guid? vendedorId, DateTime? inicio, DateTime? fim, bool count = false)
    {
        var sql = new StringBuilder(count
            ? "SELECT COUNT(*) FROM pedidos_venda WHERE tenant_id = @tenantId AND deleted_at IS NULL"
            : $"SELECT {Cols} FROM pedidos_venda WHERE tenant_id = @tenantId AND deleted_at IS NULL");
        var p = new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId };
        if (status.HasValue) { sql.Append(" AND status = @s"); p["@s"] = (int)status.Value; }
        if (clienteId.HasValue) { sql.Append(" AND cliente_id = @c"); p["@c"] = clienteId.Value; }
        if (vendedorId.HasValue) { sql.Append(" AND vendedor_id = @v"); p["@v"] = vendedorId.Value; }
        if (inicio.HasValue) { sql.Append(" AND data_emissao >= @ini"); p["@ini"] = inicio.Value; }
        if (fim.HasValue) { sql.Append(" AND data_emissao <= @fim"); p["@fim"] = fim.Value; }
        return (sql, p);
    }

    public Task<IReadOnlyList<PedidoVendaItem>> ListItensAsync(Guid pedidoId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(
            $"SELECT {ItemCols} FROM pedido_venda_itens WHERE tenant_id = @tenantId AND pedido_venda_id = @pid AND deleted_at IS NULL",
            MapItem,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@pid"] = pedidoId },
            cancellationToken);

    public async Task AddItensAsync(IEnumerable<PedidoVendaItem> itens, CancellationToken cancellationToken = default)
    {
        foreach (var i in itens)
        {
            await Db.ExecuteAsync(@"
                INSERT INTO pedido_venda_itens
                (id, tenant_id, pedido_venda_id, produto_id, quantidade, quantidade_faturada, preco_unitario,
                 created_at, created_by)
                VALUES (@id, @tenant_id, @pid, @prod, @qtd, @qf, @preco, @created_at, @created_by)",
                new Dictionary<string, object?>
                {
                    ["@id"] = i.Id,
                    ["@tenant_id"] = TenantContext.TenantId,
                    ["@pid"] = i.PedidoVendaId,
                    ["@prod"] = i.ProdutoId,
                    ["@qtd"] = i.Quantidade,
                    ["@qf"] = i.QuantidadeFaturada,
                    ["@preco"] = i.PrecoUnitario,
                    ["@created_at"] = i.CreatedAt,
                    ["@created_by"] = i.CreatedBy
                }, cancellationToken);
        }
    }

    public async Task<int> NextNumeroAsync(CancellationToken cancellationToken = default)
    {
        var c = await Db.ExecuteScalarAsync<long>(
            "SELECT COUNT(*) FROM pedidos_venda WHERE tenant_id = @tenantId",
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId }, cancellationToken);
        return (int)c + 1;
    }

    private static PedidoVenda MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Numero = r.GetValueOrDefault<string>("numero") ?? string.Empty,
        ClienteId = r.GetValueOrDefault<Guid>("cliente_id"),
        VendedorId = r.GetValueOrDefault<Guid?>("vendedor_id"),
        OrcamentoId = r.GetValueOrDefault<Guid?>("orcamento_id"),
        DataEmissao = r.GetValueOrDefault<DateTime>("data_emissao"),
        EstoqueId = r.GetValueOrDefault<Guid>("estoque_id"),
        ValorTotal = r.GetValueOrDefault<decimal>("valor_total"),
        DescontoPercentual = r.GetValueOrDefault<decimal?>("desconto_percentual"),
        Status = (StatusPedidoVenda)r.GetValueOrDefault<int>("status"),
        CondicaoPagamento = r.GetValueOrDefault<string>("condicao_pagamento"),
        Observacao = r.GetValueOrDefault<string>("observacao"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };

    private static PedidoVendaItem MapItem(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        PedidoVendaId = r.GetValueOrDefault<Guid>("pedido_venda_id"),
        ProdutoId = r.GetValueOrDefault<Guid>("produto_id"),
        Quantidade = r.GetValueOrDefault<decimal>("quantidade"),
        QuantidadeFaturada = r.GetValueOrDefault<decimal>("quantidade_faturada"),
        PrecoUnitario = r.GetValueOrDefault<decimal>("preco_unitario"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
