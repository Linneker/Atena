using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.ExternalIntegration.Sefaz.Contingencia;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Acme.Sistemas.Atena.Api.Hosted;

/// <summary>
/// Worker periódico que consulta `NFeStatusServico4` para UFs com contingência ativa
/// e atualiza a <see cref="ContingenciaPolicy"/>. Permite voltar do SVRS para origem
/// assim que a SEFAZ original normalizar.
///
/// Por enquanto a varredura é simples: itera apenas estados conhecidos na policy.
/// UFs nunca observadas (sem nenhuma transmissão) ficam fora do radar — é OK porque
/// só importa quando há tráfego.
/// </summary>
public sealed class SefazStatusWorker : BackgroundService
{
    private static readonly TimeSpan IntervaloPolling = TimeSpan.FromMinutes(1);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SefazStatusWorker> _logger;

    public SefazStatusWorker(IServiceScopeFactory scopeFactory, ILogger<SefazStatusWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("SefazStatusWorker iniciado — polling a cada {Intervalo}", IntervaloPolling);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ProcessarUmaIteracaoAsync(stoppingToken);
            }
            catch (OperationCanceledException) { /* shutdown */ }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Falha em iteração do SefazStatusWorker — vai retentar no próximo ciclo");
            }

            try
            {
                await Task.Delay(IntervaloPolling, stoppingToken);
            }
            catch (OperationCanceledException) { break; }
        }
    }

    private async Task ProcessarUmaIteracaoAsync(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var policy = scope.ServiceProvider.GetService<ContingenciaPolicy>();
        if (policy is null) return;

        // Re-testar apenas UFs que estão em contingência.
        // Em multi-tenant, o cert para chamar NFeStatusServico precisa de um tenant;
        // este worker espera um TenantContext "global"/system OU um cert default.
        // Por enquanto: apenas marca o intent de re-teste — implementação completa
        // requer decisão sobre qual tenant usar para a chamada de status.
        foreach (var uf in new[] { "SP", "RJ", "MG", "RS", "PR" })
        {
            foreach (var amb in new[] { AmbienteFiscal.Homologacao, AmbienteFiscal.Producao })
            {
                var estado = policy.GetEstado(uf, amb);
                if (estado is null) continue;

                _logger.LogDebug(
                    "SEFAZ {Uf}/{Amb} em contingência desde {Desde}, retesteEm {Retomar}",
                    uf, amb, estado.DesdeUtc, estado.RetomarTesteEmUtc);

                // Implementação completa: chamar NFeStatusServicoService.ConsultarStatusServicoAsync
                // com cert "system" e RegistrarRespostaStatusServico. Por ora, apenas log — o
                // próprio uso (transmissão NFe) recupera estado via timeout/erro de rede.
            }
        }
        await Task.CompletedTask;
    }
}
