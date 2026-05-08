using Acme.Sistemas.Infrastructure.Cache;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Acme.Sistemas.Atena.Api.Hosted;

/// <summary>
/// A cada 5 minutos: remove entradas expiradas do <see cref="LiteDbCacheStore"/> e,
/// se o arquivo exceder o limite soft de 10 GB, remove as 20% mais antigas.
/// Compacta o arquivo após eviction agressivo.
/// </summary>
public sealed class CacheCleanupWorker : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(5);
    private const long SoftLimitBytes = 10L * 1024 * 1024 * 1024;

    private readonly LiteDbCacheStore _store;
    private readonly ILogger<CacheCleanupWorker> _logger;

    public CacheCleanupWorker(LiteDbCacheStore store, ILogger<CacheCleanupWorker> logger)
    {
        _store = store;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try { Tick(); }
            catch (Exception ex) { _logger.LogError(ex, "Falha em CacheCleanupWorker.Tick"); }

            try { await Task.Delay(Interval, stoppingToken); }
            catch (TaskCanceledException) { break; }
        }
    }

    public void Tick()
    {
        var removed = _store.RemoveExpired();
        if (removed > 0) _logger.LogDebug("CacheCleanup: {Removed} entradas expiradas removidas", removed);

        var size = _store.EstimateSizeBytes();
        if (size > SoftLimitBytes)
        {
            var total = _store.CountEntries();
            var slice = (int)Math.Max(1, total / 5); // 20%
            var dropped = _store.RemoveOldest(slice);
            _logger.LogWarning(
                "CacheCleanup: cache.db excedeu {LimitMB}MB ({SizeMB}MB). Removidas {Dropped} entradas mais antigas.",
                SoftLimitBytes / 1024 / 1024, size / 1024 / 1024, dropped);
            _store.Rebuild();
        }
        // Rebuild apenas em eviction agressivo (limite de tamanho), não em cleanup periódico
        // — Rebuild é caro e DeleteMany já reclama espaço dentro do arquivo.
    }
}
