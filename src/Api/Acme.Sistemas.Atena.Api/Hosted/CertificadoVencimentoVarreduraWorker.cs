using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Settings;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Services.V1.Fiscal.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Acme.Sistemas.Atena.Api.Hosted;

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
