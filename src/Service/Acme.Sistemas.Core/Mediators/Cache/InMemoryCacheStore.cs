using System.Collections.Concurrent;
using Acme.Sistemas.Domain.Interfaces.Cache;

namespace Acme.Sistemas.Core.Mediators.Cache;

/// <summary>
/// Mock provisório de <see cref="ICacheStore"/> em memória.
/// Substituído por HybridCacheStore (LiteDB + IMemoryCache) na Fase 4.
/// </summary>
public sealed class InMemoryCacheStore : ICacheStore
{
    private sealed record Entry(object? Value, DateTime ExpiresAt);

    private readonly ConcurrentDictionary<string, Entry> _store = new();

    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        if (_store.TryGetValue(key, out var entry))
        {
            if (entry.ExpiresAt > DateTime.UtcNow)
            {
                return Task.FromResult((T?)entry.Value);
            }
            _store.TryRemove(key, out _);
        }
        return Task.FromResult(default(T?));
    }

    public Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken cancellationToken = default)
    {
        _store[key] = new Entry(value, DateTime.UtcNow.Add(ttl));
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        _store.TryRemove(key, out _);
        return Task.CompletedTask;
    }
}
