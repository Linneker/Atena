using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Financeiro;

public sealed class RelatoriosFinanceirosRepository : IRelatoriosFinanceirosRepository
{
    private readonly IDataConfiguration _db;
    private readonly ITenantContext _tenantContext;

    public RelatoriosFinanceirosRepository(IDataConfiguration db, ITenantContext tenantContext)
    {
        _db = db;
        _tenantContext = tenantContext;
    }

    public async Task<IReadOnlyDictionary<Guid, decimal>> AggregateContasPagarPorPlanoAsync(
        DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
    {
        var rows = await _db.QueryAsync(@"
            SELECT plano_de_contas_id AS id, COALESCE(SUM(valor_pago), 0) AS total
            FROM contas_pagar
            WHERE tenant_id = @tenantId AND deleted_at IS NULL
              AND plano_de_contas_id IS NOT NULL
              AND data_pagamento IS NOT NULL
              AND data_pagamento >= @inicio AND data_pagamento <= @fim
            GROUP BY plano_de_contas_id",
            r => (Id: r.GetValueOrDefault<Guid>("id"), Total: r.GetValueOrDefault<decimal>("total")),
            new Dictionary<string, object?>
            {
                ["@tenantId"] = _tenantContext.TenantId,
                ["@inicio"] = inicio,
                ["@fim"] = fim
            }, cancellationToken);

        return rows.ToDictionary(x => x.Id, x => x.Total);
    }

    public async Task<IReadOnlyDictionary<Guid, decimal>> AggregateContasReceberPorPlanoAsync(
        DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
    {
        var rows = await _db.QueryAsync(@"
            SELECT plano_de_contas_id AS id, COALESCE(SUM(valor_recebido), 0) AS total
            FROM contas_receber
            WHERE tenant_id = @tenantId AND deleted_at IS NULL
              AND plano_de_contas_id IS NOT NULL
              AND data_recebimento IS NOT NULL
              AND data_recebimento >= @inicio AND data_recebimento <= @fim
            GROUP BY plano_de_contas_id",
            r => (Id: r.GetValueOrDefault<Guid>("id"), Total: r.GetValueOrDefault<decimal>("total")),
            new Dictionary<string, object?>
            {
                ["@tenantId"] = _tenantContext.TenantId,
                ["@inicio"] = inicio,
                ["@fim"] = fim
            }, cancellationToken);

        return rows.ToDictionary(x => x.Id, x => x.Total);
    }

    public Task<decimal> TotalContasReceberPendentesAsync(CancellationToken cancellationToken = default)
        => _db.ExecuteScalarAsync<decimal>(@"
            SELECT COALESCE(SUM(valor_original - valor_recebido), 0)
            FROM contas_receber
            WHERE tenant_id = @tenantId AND deleted_at IS NULL
              AND status IN (@sP, @sPP)",
            new Dictionary<string, object?>
            {
                ["@tenantId"] = _tenantContext.TenantId,
                ["@sP"] = (int)StatusConta.Pendente,
                ["@sPP"] = (int)StatusConta.PagoParcial
            }, cancellationToken);

    public Task<decimal> TotalContasPagarPendentesAsync(CancellationToken cancellationToken = default)
        => _db.ExecuteScalarAsync<decimal>(@"
            SELECT COALESCE(SUM(valor_original - valor_pago), 0)
            FROM contas_pagar
            WHERE tenant_id = @tenantId AND deleted_at IS NULL
              AND status IN (@sP, @sPP)",
            new Dictionary<string, object?>
            {
                ["@tenantId"] = _tenantContext.TenantId,
                ["@sP"] = (int)StatusConta.Pendente,
                ["@sPP"] = (int)StatusConta.PagoParcial
            }, cancellationToken);

    public Task<decimal> TotalDividasAbertasAsync(CancellationToken cancellationToken = default)
        => _db.ExecuteScalarAsync<decimal>(@"
            SELECT COALESCE(SUM(valor_original - valor_pago), 0)
            FROM dividas
            WHERE tenant_id = @tenantId AND deleted_at IS NULL
              AND status IN (@sP, @sPP)",
            new Dictionary<string, object?>
            {
                ["@tenantId"] = _tenantContext.TenantId,
                ["@sP"] = (int)StatusConta.Pendente,
                ["@sPP"] = (int)StatusConta.PagoParcial
            }, cancellationToken);
}
