using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Acme.Sistemas.Atena.Api.Hosted;

/// <summary>
/// Job noturno (24h) que detecta gaps na cadeia NSR por (tenant, empresa) — comparando
/// o número de comprovantes existentes com o último NSR emitido. Portaria 671/2021 proíbe
/// gaps; quando há, registra alerta para o admin do tenant.
/// </summary>
public sealed class JobAuditarGapsNsrWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<JobAuditarGapsNsrWorker> _logger;
    private static readonly TimeSpan Intervalo = TimeSpan.FromHours(24);

    public JobAuditarGapsNsrWorker(IServiceScopeFactory scopeFactory, ILogger<JobAuditarGapsNsrWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try { await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); }
        catch (TaskCanceledException) { return; }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await VarrerAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha na auditoria de gaps NSR.");
            }

            try { await Task.Delay(Intervalo, stoppingToken); }
            catch (TaskCanceledException) { return; }
        }
    }

    private async Task VarrerAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<IDataConfiguration>();

        var rows = await db.QueryAsync(@"
            SELECT n.tenant_id, n.empresa_id, n.ultimo_numero,
                   COALESCE((SELECT COUNT(*) FROM comprovantes_ponto c
                             WHERE c.tenant_id = n.tenant_id AND c.empresa_id = n.empresa_id
                               AND c.deleted_at IS NULL), 0) AS total
            FROM numerador_nsr n;",
            r => (
                TenantId: Guid.Parse(r.GetString(0)),
                EmpresaId: Guid.Parse(r.GetString(1)),
                UltimoNumero: r.GetInt64(2),
                Total: r.GetInt64(3)),
            new Dictionary<string, object?>(),
            cancellationToken);

        foreach (var (tenantId, empresaId, ultimo, total) in rows)
        {
            if (ultimo > 0 && total < ultimo)
            {
                var gap = ultimo - total;
                _logger.LogWarning(
                    "NSR gap detectado: tenant={Tenant} empresa={Empresa} ultimo={Ultimo} comprovantes={Total} gap={Gap}",
                    tenantId, empresaId, ultimo, total, gap);
            }
        }
    }
}
