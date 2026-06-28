using Acme.Sistemas.Domain.Entities.Auditoria;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Services.V1.Rh.Ponto.Engine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Acme.Sistemas.Atena.Api.Hosted;

/// <summary>
/// Job noturno que varre todas as cadeias de marcações de ponto (por funcionário)
/// e detecta adulteração via hash-chain. Grava AuditLog quando detecta quebra.
/// </summary>
public sealed class JobVerificarIntegridadePontoWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<JobVerificarIntegridadePontoWorker> _logger;
    private static readonly TimeSpan Intervalo = TimeSpan.FromHours(24);

    public JobVerificarIntegridadePontoWorker(
        IServiceScopeFactory scopeFactory,
        ILogger<JobVerificarIntegridadePontoWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Aguarda 5min após boot para não competir com migrations
        try { await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); }
        catch (TaskCanceledException) { return; }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await VerificarAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha na varredura de integridade do ponto.");
            }

            try { await Task.Delay(Intervalo, stoppingToken); }
            catch (TaskCanceledException) { return; }
        }
    }

    private async Task VerificarAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var sp = scope.ServiceProvider;
        var marcacoes = sp.GetRequiredService<IMarcacaoPontoRepository>();
        var auditLog = sp.GetService<IAuditLogRepository>();

        var funcionarioIds = await marcacoes.ListFuncionarioIdsComMarcacoesAsync(cancellationToken);
        var totalQuebras = 0;

        foreach (var fid in funcionarioIds)
        {
            if (cancellationToken.IsCancellationRequested) return;

            var lista = await marcacoes.ListAllByFuncionarioOrdenadasAsync(fid, cancellationToken);
            var tuplas = lista
                .Select(m => (m.Id, m.FuncionarioId, m.DataHora, m.Tipo, m.Origem, m.HashAnterior, m.HashIntegridade))
                .ToList();

            var quebra = MarcacaoPontoIntegridade.VerificarCadeia(tuplas);
            if (quebra is not null)
            {
                totalQuebras++;
                _logger.LogWarning(
                    "MarcacaoPontoIntegridadeViolada: funcionario={FuncionarioId} marcacao={MarcacaoId} indice={Indice} tipo={TipoQuebra}",
                    fid, quebra.MarcacaoId, quebra.Indice, quebra.TipoQuebra);

                if (auditLog is not null)
                {
                    await auditLog.AddAsync(new AuditLog
                    {
                        EntidadeNome = "MarcacaoPonto",
                        EntidadeId = quebra.MarcacaoId,
                        CommandTipo = "MarcacaoPontoIntegridadeViolada",
                        Operacao = OperacaoAuditoria.Outro,
                        DepoisJson = System.Text.Json.JsonSerializer.Serialize(quebra),
                        OcorridoEm = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow,
                    }, cancellationToken);
                }
            }
        }

        _logger.LogInformation(
            "Varredura de integridade do ponto concluída. Funcionários verificados: {Total}, quebras: {Quebras}.",
            funcionarioIds.Count, totalQuebras);
    }
}
