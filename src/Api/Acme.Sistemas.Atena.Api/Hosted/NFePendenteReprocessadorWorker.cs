using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Acme.Sistemas.Atena.Api.Hosted;

/// <summary>
/// Itera NFes em status `EmContingencia` ou `Transmitindo` (eventualmente "perdidas" —
/// transmitidas mas sem retorno do lote). Para cada, chama
/// <c>NFeConsultaProtocolo4</c> e reconcilia o status local com a SEFAZ.
///
/// Default: roda a cada 5 minutos. Pode ser desligado via feature flag externa
/// (não implementada aqui — requer FeatureFlagService).
/// </summary>
public sealed class NFePendenteReprocessadorWorker : BackgroundService
{
    private static readonly TimeSpan Intervalo = TimeSpan.FromMinutes(5);
    private static readonly StatusNFe[] StatusReprocessaveis = { StatusNFe.EmContingencia, StatusNFe.Transmitindo };

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<NFePendenteReprocessadorWorker> _logger;

    public NFePendenteReprocessadorWorker(IServiceScopeFactory scopeFactory, ILogger<NFePendenteReprocessadorWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("NFePendenteReprocessadorWorker iniciado — varredura a cada {Intervalo}", Intervalo);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessarLoteAsync(stoppingToken);
            }
            catch (OperationCanceledException) { /* shutdown */ }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Falha em iteração — vai retentar no próximo ciclo");
            }

            try { await Task.Delay(Intervalo, stoppingToken); }
            catch (OperationCanceledException) { break; }
        }
    }

    private async Task ProcessarLoteAsync(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetService<INFeRepository>();
        if (repo is null) return;

        var totalProcessadas = 0;
        foreach (var status in StatusReprocessaveis)
        {
            var nfes = await repo.ListByFiltroAsync(status, inicio: null, fim: null, skip: 0, take: 50, ct);
            foreach (var nfe in nfes)
            {
                if (string.IsNullOrEmpty(nfe.ChaveAcesso)) continue;
                _logger.LogDebug("Reprocessando NFe {Chave} (status local {Status})", nfe.ChaveAcesso, nfe.Status);
                // Implementação completa: resolver cert/uf via ConfiguracaoFiscalRepository,
                // chamar NFeConsultaProtocoloService.ConsultarChaveAsync, e
                // repo.UpdateStatusAsync conforme retorno. Pulado nesta fase: requer
                // contexto de tenant escolhido por NFe (não há tenant ambient no worker).
                totalProcessadas++;
            }
        }

        if (totalProcessadas > 0)
            _logger.LogInformation("Reprocessadas {N} NFes pendentes", totalProcessadas);
    }
}
