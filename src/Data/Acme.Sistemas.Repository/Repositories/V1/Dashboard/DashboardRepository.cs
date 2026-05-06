using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Dashboard;

public sealed class DashboardRepository : IDashboardRepository
{
    private readonly IDataConfiguration _db;
    private readonly ITenantContext _tenantContext;

    public DashboardRepository(IDataConfiguration db, ITenantContext tenantContext)
    {
        _db = db;
        _tenantContext = tenantContext;
    }

    public async Task<int> CountVendasAbertasAsync(CancellationToken cancellationToken = default)
    {
        var c = await _db.ExecuteScalarAsync<long>(@"
            SELECT COUNT(*) FROM pedidos_venda
            WHERE tenant_id = @tenantId AND deleted_at IS NULL
              AND status IN (@s1, @s2, @s3)",
            new Dictionary<string, object?>
            {
                ["@tenantId"] = _tenantContext.TenantId,
                ["@s1"] = 0, // Rascunho
                ["@s2"] = 1, // Confirmado
                ["@s3"] = 2  // FaturamentoParcial
            }, cancellationToken);
        return (int)c;
    }

    public async Task<int> CountContasReceberVencendoAsync(int diasJanela, CancellationToken cancellationToken = default)
    {
        var c = await _db.ExecuteScalarAsync<long>(@"
            SELECT COUNT(*) FROM contas_receber
            WHERE tenant_id = @tenantId AND deleted_at IS NULL
              AND status IN (@sP, @sPP)
              AND data_vencimento BETWEEN CURDATE() AND DATE_ADD(CURDATE(), INTERVAL @dias DAY)",
            new Dictionary<string, object?>
            {
                ["@tenantId"] = _tenantContext.TenantId,
                ["@sP"] = (int)StatusConta.Pendente,
                ["@sPP"] = (int)StatusConta.PagoParcial,
                ["@dias"] = diasJanela
            }, cancellationToken);
        return (int)c;
    }

    public async Task<int> CountContasPagarVencendoAsync(int diasJanela, CancellationToken cancellationToken = default)
    {
        var c = await _db.ExecuteScalarAsync<long>(@"
            SELECT COUNT(*) FROM contas_pagar
            WHERE tenant_id = @tenantId AND deleted_at IS NULL
              AND status IN (@sP, @sPP)
              AND data_vencimento BETWEEN CURDATE() AND DATE_ADD(CURDATE(), INTERVAL @dias DAY)",
            new Dictionary<string, object?>
            {
                ["@tenantId"] = _tenantContext.TenantId,
                ["@sP"] = (int)StatusConta.Pendente,
                ["@sPP"] = (int)StatusConta.PagoParcial,
                ["@dias"] = diasJanela
            }, cancellationToken);
        return (int)c;
    }

    public async Task<int> CountProdutosEmEstoqueCriticoAsync(CancellationToken cancellationToken = default)
    {
        var c = await _db.ExecuteScalarAsync<long>(@"
            SELECT COUNT(DISTINCT p.id)
            FROM produtos p
            INNER JOIN estoque_produtos ep ON ep.produto_id = p.id AND ep.tenant_id = p.tenant_id AND ep.deleted_at IS NULL
            WHERE p.tenant_id = @tenantId AND p.deleted_at IS NULL
              AND p.estoque_minimo IS NOT NULL
              AND (ep.saldo_total - ep.saldo_reservado) <= p.estoque_minimo",
            new Dictionary<string, object?> { ["@tenantId"] = _tenantContext.TenantId },
            cancellationToken);
        return (int)c;
    }

    public Task<IReadOnlyList<(int Ano, int Mes, decimal Receitas, decimal Despesas)>> EvolucaoFinanceiraUltimosMesesAsync(
        int meses, CancellationToken cancellationToken = default)
        => _db.QueryAsync(@"
            SELECT
              YEAR(periodo) AS ano,
              MONTH(periodo) AS mes,
              COALESCE(SUM(receita), 0) AS receita,
              COALESCE(SUM(despesa), 0) AS despesa
            FROM (
              SELECT data_recebimento AS periodo, valor_recebido AS receita, 0 AS despesa
              FROM contas_receber
              WHERE tenant_id = @tenantId AND deleted_at IS NULL
                AND data_recebimento IS NOT NULL
                AND data_recebimento >= DATE_SUB(CURDATE(), INTERVAL @meses MONTH)
              UNION ALL
              SELECT data_pagamento AS periodo, 0 AS receita, valor_pago AS despesa
              FROM contas_pagar
              WHERE tenant_id = @tenantId AND deleted_at IS NULL
                AND data_pagamento IS NOT NULL
                AND data_pagamento >= DATE_SUB(CURDATE(), INTERVAL @meses MONTH)
            ) AS movimentos
            GROUP BY YEAR(periodo), MONTH(periodo)
            ORDER BY ano, mes",
            r => (
                Ano: r.GetValueOrDefault<int>("ano"),
                Mes: r.GetValueOrDefault<int>("mes"),
                Receitas: r.GetValueOrDefault<decimal>("receita"),
                Despesas: r.GetValueOrDefault<decimal>("despesa")),
            new Dictionary<string, object?>
            {
                ["@tenantId"] = _tenantContext.TenantId,
                ["@meses"] = meses
            }, cancellationToken);
}
