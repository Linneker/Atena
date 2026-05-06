using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Settings;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Services.V1.Fiscal.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Acme.Sistemas.Infrastructure.Hosted;

/// <summary>
/// Varredura periódica que detecta certificados fiscais a vencer (default: 30 dias)
/// e publica <see cref="CertificadoAVencerNotification"/> para cada tenant afetado.
/// </summary>
public sealed class CertificadoVencimentoVarreduraWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<CertificadoVencimentoVarreduraWorker> _logger;
    private readonly FiscalOptions _options;

    public CertificadoVencimentoVarreduraWorker(
        IServiceScopeFactory scopeFactory,
        IOptions<FiscalOptions> options,
        ILogger<CertificadoVencimentoVarreduraWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var intervalo = TimeSpan.FromHours(Math.Max(1, _options.IntervaloVarreduraCertificadosHoras));
        // Espera 60s antes da primeira varredura para dar tempo de migrations etc.
        try { await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken); }
        catch (TaskCanceledException) { return; }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await VarrerAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha na varredura de certificados a vencer.");
            }

            try { await Task.Delay(intervalo, stoppingToken); }
            catch (TaskCanceledException) { return; }
        }
    }

    private async Task VarrerAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var query = scope.ServiceProvider.GetRequiredService<IConfiguracoesFiscaisQueryRepository>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var dias = Math.Max(1, _options.DiasAlertaCertificadoVencer);
        var limite = DateTime.UtcNow.AddDays(dias);
        var lista = await query.ListarComCertificadoVencendoAsync(limite, cancellationToken);

        _logger.LogInformation("Varredura de certificados: {Count} a vencer em {Dias} dias.", lista.Count, dias);

        var agora = DateTime.UtcNow;
        foreach (var c in lista)
        {
            if (!c.CertificadoValidoAte.HasValue) continue;
            var diasRestantes = (int)Math.Round((c.CertificadoValidoAte.Value - agora).TotalDays);
            await mediator.Publish(new CertificadoAVencerNotification(
                c.TenantId, c.CertificadoSubject, c.CertificadoValidoAte.Value, diasRestantes, agora),
                cancellationToken);
        }
    }
}
