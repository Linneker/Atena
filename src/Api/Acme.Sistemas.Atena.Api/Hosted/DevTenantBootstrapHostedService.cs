using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Settings;
using Acme.Sistemas.Services.V1.Admin.Command.SeedTenant;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Microsoft.Extensions.Options;

namespace Acme.Sistemas.Atena.Api.Hosted;

/// <summary>
/// Em ambiente Development, se <c>Seed:AutoBootstrap=true</c> e não houver nenhum tenant,
/// cria o tenant <c>demo@atena.test</c> automaticamente no boot, logando a senha gerada no
/// console. No-op em Production (proteção dupla: registro condicional em Program.cs +
/// verificação de ambiente aqui).
/// </summary>
public sealed class DevTenantBootstrapHostedService : BackgroundService
{
    private const string DemoCnpj = "00000000000191";
    private const string DemoRazao = "Atena Demo Ltda";
    private const string DemoEmail = "demo@atena.test";

    private readonly IServiceProvider _services;
    private readonly IHostEnvironment _environment;
    private readonly IOptions<SeedOptions> _seed;
    private readonly ILogger<DevTenantBootstrapHostedService> _logger;

    public DevTenantBootstrapHostedService(
        IServiceProvider services,
        IHostEnvironment environment,
        IOptions<SeedOptions> seed,
        ILogger<DevTenantBootstrapHostedService> logger)
    {
        _services = services;
        _environment = environment;
        _seed = seed;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Proteção dupla: nunca roda fora de Development, mesmo com a flag ligada.
        if (!_environment.IsDevelopment() || !_seed.Value.AutoBootstrap)
            return;

        try
        {
            using var scope = _services.CreateScope();
            var tenants = scope.ServiceProvider.GetRequiredService<ITenantRepository>();

            var existentes = await tenants.ListAsync(0, 1, stoppingToken);
            if (existentes.Count > 0)
            {
                _logger.LogInformation("DevTenantBootstrap: já existe tenant — bootstrap ignorado.");
                return;
            }

            // Aguarda o PermissionsSeedHostedService popular as permissões (ambos são
            // BackgroundService e iniciam concorrentemente) para que as roles nasçam com permissões.
            await AguardarPermissoesAsync(scope.ServiceProvider, stoppingToken);

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var result = await mediator.Send(
                new SeedTenantCommand(DemoCnpj, DemoRazao, DemoEmail), stoppingToken);

            if (result.IsSuccess && result.Content is { EhNovo: true } c)
            {
                _logger.LogWarning(
                    "DevTenantBootstrap: tenant demo criado. Login: {Email} / Senha: {Senha} (TenantId={TenantId})",
                    DemoEmail, c.SenhaInicial, c.TenantId);
            }
            else
            {
                _logger.LogInformation("DevTenantBootstrap: nada a fazer (tenant já existente ou falha controlada).");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "DevTenantBootstrap falhou (banco indisponível?). Tentará no próximo boot.");
        }
    }

    private static async Task AguardarPermissoesAsync(IServiceProvider sp, CancellationToken ct)
    {
        var permissions = sp.GetRequiredService<IPermissionRepository>();
        for (var i = 0; i < 30 && !ct.IsCancellationRequested; i++)
        {
            var all = await permissions.ListAllAsync(ct);
            if (all.Count > 0) return;
            await Task.Delay(500, ct);
        }
    }
}
