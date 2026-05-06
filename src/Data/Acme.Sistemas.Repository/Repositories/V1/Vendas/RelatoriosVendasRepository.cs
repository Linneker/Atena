using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Vendas;

public sealed class RelatoriosVendasRepository : IRelatoriosVendasRepository
{
    private readonly IDataConfiguration _db;
    private readonly ITenantContext _tenantContext;

    public RelatoriosVendasRepository(IDataConfiguration db, ITenantContext tenantContext)
    {
        _db = db;
        _tenantContext = tenantContext;
    }

    public async Task<IReadOnlyList<(Guid VendedorId, decimal Total, int Faturamentos)>> AgruparPorVendedorAsync(
        DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
    {
        var rows = await _db.QueryAsync(@"
            SELECT pv.vendedor_id AS vid,
                   COALESCE(SUM(f.valor_total), 0) AS total,
                   COUNT(f.id) AS qtd
            FROM faturamentos f
            INNER JOIN pedidos_venda pv ON pv.id = f.pedido_venda_id
            WHERE f.tenant_id = @tenantId AND f.deleted_at IS NULL
              AND pv.vendedor_id IS NOT NULL
              AND f.data_faturamento >= @ini AND f.data_faturamento <= @fim
            GROUP BY pv.vendedor_id
            ORDER BY total DESC",
            r => (
                VendedorId: r.GetValueOrDefault<Guid>("vid"),
                Total: r.GetValueOrDefault<decimal>("total"),
                Faturamentos: r.GetValueOrDefault<int>("qtd")),
            new Dictionary<string, object?>
            {
                ["@tenantId"] = _tenantContext.TenantId,
                ["@ini"] = inicio,
                ["@fim"] = fim
            }, cancellationToken);
        return rows;
    }

    public async Task<IReadOnlyList<(Guid ClienteId, decimal Total, int Faturamentos)>> AgruparPorClienteAsync(
        DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
    {
        var rows = await _db.QueryAsync(@"
            SELECT pv.cliente_id AS cid,
                   COALESCE(SUM(f.valor_total), 0) AS total,
                   COUNT(f.id) AS qtd
            FROM faturamentos f
            INNER JOIN pedidos_venda pv ON pv.id = f.pedido_venda_id
            WHERE f.tenant_id = @tenantId AND f.deleted_at IS NULL
              AND f.data_faturamento >= @ini AND f.data_faturamento <= @fim
            GROUP BY pv.cliente_id
            ORDER BY total DESC",
            r => (
                ClienteId: r.GetValueOrDefault<Guid>("cid"),
                Total: r.GetValueOrDefault<decimal>("total"),
                Faturamentos: r.GetValueOrDefault<int>("qtd")),
            new Dictionary<string, object?>
            {
                ["@tenantId"] = _tenantContext.TenantId,
                ["@ini"] = inicio,
                ["@fim"] = fim
            }, cancellationToken);
        return rows;
    }

    public async Task<IReadOnlyList<(Guid ProdutoId, decimal Quantidade, decimal Total)>> AgruparPorProdutoAsync(
        DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
    {
        var rows = await _db.QueryAsync(@"
            SELECT fi.produto_id AS pid,
                   COALESCE(SUM(fi.quantidade), 0) AS qtd,
                   COALESCE(SUM(fi.quantidade * fi.preco_unitario), 0) AS total
            FROM faturamento_itens fi
            INNER JOIN faturamentos f ON f.id = fi.faturamento_id
            WHERE fi.tenant_id = @tenantId AND fi.deleted_at IS NULL
              AND f.data_faturamento >= @ini AND f.data_faturamento <= @fim
            GROUP BY fi.produto_id
            ORDER BY total DESC",
            r => (
                ProdutoId: r.GetValueOrDefault<Guid>("pid"),
                Quantidade: r.GetValueOrDefault<decimal>("qtd"),
                Total: r.GetValueOrDefault<decimal>("total")),
            new Dictionary<string, object?>
            {
                ["@tenantId"] = _tenantContext.TenantId,
                ["@ini"] = inicio,
                ["@fim"] = fim
            }, cancellationToken);
        return rows;
    }
}
