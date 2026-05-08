using Acme.Sistemas.Domain.Interfaces.Cache;
using Microsoft.Extensions.Caching.Memory;

namespace Acme.Sistemas.Infrastructure.Cache;

/// <summary>
/// L1 = <see cref="IMemoryCache"/> (hot, in-process).
/// L2 = <see cref="LiteDbCacheStore"/> (cold, single-file persistente).
/// Política: get → memory → litedb → miss; set → grava nas duas; remove → remove das duas.
/// TTL respeita o valor recebido (default 15 min é responsabilidade de quem chama).
/// </summary>
public sealed class HybridCacheStore : ICacheStore
{
    public static readonly TimeSpan DefaultTtl = TimeSpan.FromMinutes(15);

    private readonly IMemoryCache _memory;
    private readonly LiteDbCacheStore _cold;

    public HybridCacheStore(IMemoryCache memory, LiteDbCacheStore cold)
    {
        _memory = memory;
        _cold = cold;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        if (_memory.TryGetValue(key, out T? hot)) return hot;

        var cold = await _cold.GetAsync<T>(key, cancellationToken);
        if (cold is not null)
        {
            // Repopula L1 com TTL default (não temos o TTL original aqui).
            _memory.Set(key, cold, DefaultTtl);
        }
        return cold;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken cancellationToken = default)
    {
        var effectiveTtl = ttl > TimeSpan.Zero ? ttl : DefaultTtl;
        _memory.Set(key, value, effectiveTtl);
        await _cold.SetAsync(key, value, effectiveTtl, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        _memory.Remove(key);
        await _cold.RemoveAsync(key, cancellationToken);
    }
}
