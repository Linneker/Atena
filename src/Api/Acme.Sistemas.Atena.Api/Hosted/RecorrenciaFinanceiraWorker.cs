using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Security;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Services.V1.Despesa.Command.GerarRecorrencias;
using Acme.Sistemas.Services.V1.Receita.Command.GerarRecorrencias;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Acme.Sistemas.Atena.Api.Hosted;

/// <summary>
/// Worker que gera lançamentos recorrentes para Despesas/Receitas marcadas como Fixas
/// em todos os tenants ativos. Roda a cada 24h e horizonte default de 3 meses.
///
/// Por que não usa filtro de "primeiro dia do mês": evita janela curta de execução
/// e funciona idempotentemente — o command interno só cria entries que ainda não existem
/// no ano-mês alvo (critério "mesmo Nome no mês destino").
/// </summary>
public sealed class RecorrenciaFinanceiraWorker : BackgroundService
{
    private static readonly TimeSpan Intervalo = TimeSpan.FromHours(24);
    private const int MesesParaFrente = 3;

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<RecorrenciaFinanceiraWorker> _logger;

    public RecorrenciaFinanceiraWorker(
        IServiceScopeFactory scopeFactory,
        ILogger<RecorrenciaFinanceiraWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "RecorrenciaFinanceiraWorker iniciado — varredura a cada {Intervalo} (horizonte {Meses} meses)",
            Intervalo, MesesParaFrente);

        // Aguarda 60s antes do primeiro tick para não competir com migrations no boot.
        try { await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken); }
        catch (OperationCanceledException) { return; }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await VarrerTodosTenantsAsync(stoppingToken);
            }
            catch (OperationCanceledException) { return; }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Falha em varredura — vai retentar no próximo ciclo");
            }

            try { await Task.Delay(Intervalo, stoppingToken); }
            catch (OperationCanceledException) { return; }
        }
    }

    private async Task VarrerTodosTenantsAsync(CancellationToken ct)
    {
        using var rootScope = _scopeFactory.CreateScope();
        var tenants = await rootScope.ServiceProvider.GetRequiredService<ITenantRepository>()
            .ListAsync(0, 1000, ct);

        var totalDespesasGeradas = 0;
        var totalReceitasGeradas = 0;

        foreach (var tenant in tenants)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                scope.ServiceProvider.GetRequiredService<IMutableTenantContext>().Override(tenant.Id);
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                var resDesp = await mediator.Send(
                    new GerarRecorrenciasDespesaCommand(MesesParaFrente), ct);
                if (resDesp.IsSuccess && resDesp.Content is not null)
                    totalDespesasGeradas += resDesp.Content.Geradas;

                var resRec = await mediator.Send(
                    new GerarRecorrenciasReceitaCommand(MesesParaFrente), ct);
                if (resRec.IsSuccess && resRec.Content is not null)
                    totalReceitasGeradas += resRec.Content.Geradas;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex,
                    "Falha ao processar recorrências do tenant {TenantId} — continua para o próximo",
                    tenant.Id);
            }
        }

        if (totalDespesasGeradas + totalReceitasGeradas > 0)
        {
            _logger.LogInformation(
                "RecorrenciaFinanceira: {Tenants} tenants varridos. Despesas geradas: {D}. Receitas geradas: {R}.",
                tenants.Count, totalDespesasGeradas, totalReceitasGeradas);
        }
    }
}
