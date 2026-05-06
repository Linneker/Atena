using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Domain.Entities.Permissions;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Atena.Api.Hosted;

public sealed class PermissionsSeedHostedService : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<PermissionsSeedHostedService> _logger;

    public PermissionsSeedHostedService(IServiceProvider services, ILogger<PermissionsSeedHostedService> logger)
    {
        _services = services;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _services.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IPermissionRepository>();

            var existing = await repo.ListAllAsync(stoppingToken);
            var existingCodes = existing.Select(p => p.Codigo).ToHashSet(StringComparer.OrdinalIgnoreCase);

            var seeded = 0;
            foreach (var codigo in Permissions.All())
            {
                if (existingCodes.Contains(codigo)) continue;
                var parts = codigo.Split(':', 2);
                await repo.UpsertAsync(new Permission
                {
                    Recurso = parts[0],
                    Acao = parts.Length > 1 ? parts[1] : string.Empty,
                    Codigo = codigo
                }, stoppingToken);
                seeded++;
            }

            if (seeded > 0)
                _logger.LogInformation("Permissions seed concluído: {Seeded} novas permissões.", seeded);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Permissions seed falhou (banco indisponível?). Será tentado novamente no próximo boot.");
        }
    }
}
