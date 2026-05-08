using Acme.Sistemas.Core.Settings;
using Acme.Sistemas.Domain.Interfaces.Cache;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Acme.Sistemas.Infrastructure.Cache;

/// <summary>
/// Roteador de provider único registrado como <see cref="ICacheStore"/>.
/// Resolve em runtime entre <see cref="HybridCacheStore"/> (LiteDb+Memory, default)
/// e <see cref="RedisCacheStore"/> (opt-in via flag <c>Cache:Provider=Redis</c>).
/// Em falha do Redis, cai automaticamente para Hybrid e emite warning.
/// Hot-swap: ler a flag a cada chamada via <see cref="IOptionsMonitor{T}"/>.
/// </summary>
public sealed class CacheProviderRouter : ICacheStore
{
    public const string ProviderRedis = "Redis";
    public const string ProviderLiteDb = "LiteDb";

    private readonly HybridCacheStore _hybrid;
    private readonly RedisCacheStore? _redis;
    private readonly IOptionsMonitor<FeatureFlagSettings> _flags;
    private readonly ILogger<CacheProviderRouter> _logger;

    public CacheProviderRouter(
        HybridCacheStore hybrid,
        IOptionsMonitor<FeatureFlagSettings> flags,
        ILogger<CacheProviderRouter> logger,
        RedisCacheStore? redis = null)
    {
        _hybrid = hybrid;
        _redis = redis;
        _flags = flags;
        _logger = logger;
    }

    private ICacheStore Resolve()
    {
        var provider = _flags.CurrentValue.Cache.Provider ?? ProviderLiteDb;
        if (string.Equals(provider, ProviderRedis, StringComparison.OrdinalIgnoreCase) && _redis is not null)
        {
            return _redis;
        }
        return _hybrid;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var store = Resolve();
        try { return await store.GetAsync<T>(key, cancellationToken); }
        catch (Exception ex) when (store is RedisCacheStore)
        {
            _logger.LogWarning(ex, "Falha no Redis em GetAsync({Key}); caindo para Hybrid.", key);
            return await _hybrid.GetAsync<T>(key, cancellationToken);
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken cancellationToken = default)
    {
        var store = Resolve();
        try { await store.SetAsync(key, value, ttl, cancellationToken); }
        catch (Exception ex) when (store is RedisCacheStore)
        {
            _logger.LogWarning(ex, "Falha no Redis em SetAsync({Key}); caindo para Hybrid.", key);
            await _hybrid.SetAsync(key, value, ttl, cancellationToken);
        }
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        var store = Resolve();
        try { await store.RemoveAsync(key, cancellationToken); }
        catch (Exception ex) when (store is RedisCacheStore)
        {
            _logger.LogWarning(ex, "Falha no Redis em RemoveAsync({Key}); caindo para Hybrid.", key);
            await _hybrid.RemoveAsync(key, cancellationToken);
        }
    }
}
