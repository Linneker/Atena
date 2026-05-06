using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Financeiro;

public sealed class AgingRepository : IAgingRepository
{
    private readonly IDataConfiguration _db;
    private readonly ITenantContext _tenantContext;

    public AgingRepository(IDataConfiguration db, ITenantContext tenantContext)
    {
        _db = db;
        _tenantContext = tenantContext;
    }

    public Task<IReadOnlyList<(string Faixa, int Quantidade, decimal Valor)>> AgingContasPagarAsync(CancellationToken cancellationToken = default)
        => _db.QueryAsync(@"
            SELECT faixa, COUNT(*) AS qtd, COALESCE(SUM(saldo), 0) AS total
            FROM (
              SELECT
                CASE
                  WHEN data_vencimento >= CURDATE() AND data_vencimento <= DATE_ADD(CURDATE(), INTERVAL 7 DAY) THEN '0-7'
                  WHEN data_vencimento > DATE_ADD(CURDATE(), INTERVAL 7 DAY) AND data_vencimento <= DATE_ADD(CURDATE(), INTERVAL 30 DAY) THEN '8-30'
                  WHEN data_vencimento > DATE_ADD(CURDATE(), INTERVAL 30 DAY) AND data_vencimento <= DATE_ADD(CURDATE(), INTERVAL 60 DAY) THEN '31-60'
                  WHEN data_vencimento > DATE_ADD(CURDATE(), INTERVAL 60 DAY) THEN 'acima-60'
                  WHEN data_vencimento < CURDATE() AND DATEDIFF(CURDATE(), data_vencimento) <= 30 THEN 'vencido-1-30'
                  WHEN data_vencimento < CURDATE() AND DATEDIFF(CURDATE(), data_vencimento) <= 60 THEN 'vencido-31-60'
                  ELSE 'vencido-acima-60'
                END AS faixa,
                (valor_original - valor_pago) AS saldo
              FROM contas_pagar
              WHERE tenant_id = @tenantId AND deleted_at IS NULL
                AND status IN (@sP, @sPP)
            ) t
            GROUP BY faixa",
            r => (
                Faixa: r.GetValueOrDefault<string>("faixa") ?? string.Empty,
                Quantidade: r.GetValueOrDefault<int>("qtd"),
                Valor: r.GetValueOrDefault<decimal>("total")),
            new Dictionary<string, object?>
            {
                ["@tenantId"] = _tenantContext.TenantId,
                ["@sP"] = (int)StatusConta.Pendente,
                ["@sPP"] = (int)StatusConta.PagoParcial
            }, cancellationToken);

    public Task<IReadOnlyList<(string Faixa, int Quantidade, decimal Valor)>> AgingContasReceberAsync(CancellationToken cancellationToken = default)
        => _db.QueryAsync(@"
            SELECT faixa, COUNT(*) AS qtd, COALESCE(SUM(saldo), 0) AS total
            FROM (
              SELECT
                CASE
                  WHEN data_vencimento >= CURDATE() AND data_vencimento <= DATE_ADD(CURDATE(), INTERVAL 7 DAY) THEN '0-7'
                  WHEN data_vencimento > DATE_ADD(CURDATE(), INTERVAL 7 DAY) AND data_vencimento <= DATE_ADD(CURDATE(), INTERVAL 30 DAY) THEN '8-30'
                  WHEN data_vencimento > DATE_ADD(CURDATE(), INTERVAL 30 DAY) AND data_vencimento <= DATE_ADD(CURDATE(), INTERVAL 60 DAY) THEN '31-60'
                  WHEN data_vencimento > DATE_ADD(CURDATE(), INTERVAL 60 DAY) THEN 'acima-60'
                  WHEN data_vencimento < CURDATE() AND DATEDIFF(CURDATE(), data_vencimento) <= 30 THEN 'vencido-1-30'
                  WHEN data_vencimento < CURDATE() AND DATEDIFF(CURDATE(), data_vencimento) <= 60 THEN 'vencido-31-60'
                  ELSE 'vencido-acima-60'
                END AS faixa,
                (valor_original - valor_recebido) AS saldo
              FROM contas_receber
              WHERE tenant_id = @tenantId AND deleted_at IS NULL
                AND status IN (@sP, @sPP)
            ) t
            GROUP BY faixa",
            r => (
                Faixa: r.GetValueOrDefault<string>("faixa") ?? string.Empty,
                Quantidade: r.GetValueOrDefault<int>("qtd"),
                Valor: r.GetValueOrDefault<decimal>("total")),
            new Dictionary<string, object?>
            {
                ["@tenantId"] = _tenantContext.TenantId,
                ["@sP"] = (int)StatusConta.Pendente,
                ["@sPP"] = (int)StatusConta.PagoParcial
            }, cancellationToken);
}
