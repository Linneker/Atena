using System.Text;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Estoque;

public sealed class PosicaoEstoqueRepository : IPosicaoEstoqueRepository
{
    private readonly IDataConfiguration _db;
    private readonly ITenantContext _tenantContext;

    public PosicaoEstoqueRepository(IDataConfiguration db, ITenantContext tenantContext)
    {
        _db = db;
        _tenantContext = tenantContext;
    }

    public Task<IReadOnlyList<PosicaoEstoqueLinha>> ConsultarAsync(Guid? estoqueId, CancellationToken cancellationToken = default)
    {
        var sql = new StringBuilder(@"
            SELECT
              p.id AS produto_id,
              p.codigo AS codigo_produto,
              p.nome AS nome_produto,
              COALESCE(SUM(ep.saldo_total), 0) AS saldo_total,
              COALESCE(SUM(ep.saldo_reservado), 0) AS saldo_reservado,
              p.custo_medio AS custo_medio
            FROM produtos p
            LEFT JOIN estoque_produtos ep
              ON ep.produto_id = p.id
              AND ep.tenant_id = p.tenant_id
              AND ep.deleted_at IS NULL
            WHERE p.tenant_id = @tenantId AND p.deleted_at IS NULL");

        var p = new Dictionary<string, object?> { ["@tenantId"] = _tenantContext.TenantId };
        if (estoqueId.HasValue)
        {
            sql.Append(" AND ep.estoque_id = @eid");
            p["@eid"] = estoqueId.Value;
        }
        sql.Append(" GROUP BY p.id, p.codigo, p.nome, p.custo_medio ORDER BY p.nome");

        return _db.QueryAsync(sql.ToString(),
            r =>
            {
                var saldoTotal = r.GetValueOrDefault<decimal>("saldo_total");
                var saldoReservado = r.GetValueOrDefault<decimal>("saldo_reservado");
                var custo = r.GetValueOrDefault<decimal?>("custo_medio");
                return new PosicaoEstoqueLinha(
                    ProdutoId: r.GetValueOrDefault<Guid>("produto_id"),
                    CodigoProduto: r.GetValueOrDefault<string>("codigo_produto") ?? string.Empty,
                    NomeProduto: r.GetValueOrDefault<string>("nome_produto") ?? string.Empty,
                    SaldoTotal: saldoTotal,
                    SaldoReservado: saldoReservado,
                    SaldoDisponivel: saldoTotal - saldoReservado,
                    CustoMedio: custo,
                    ValorEstoque: (custo ?? 0) * saldoTotal);
            }, p, cancellationToken);
    }
}
