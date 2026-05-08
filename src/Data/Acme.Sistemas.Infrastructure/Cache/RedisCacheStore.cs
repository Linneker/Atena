using System.Text.Json;
using Acme.Sistemas.Domain.Interfaces.Cache;
using StackExchange.Redis;

namespace Acme.Sistemas.Infrastructure.Cache;

public sealed class RedisCacheStore : ICacheStore
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    private readonly IConnectionMultiplexer _mux;

    public RedisCacheStore(IConnectionMultiplexer mux)
    {
        _mux = mux;
    }

    private IDatabase Db => _mux.GetDatabase();

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var value = await Db.StringGetAsync(key);
        if (value.IsNullOrEmpty) return default;
        try { return JsonSerializer.Deserialize<T>((string)value!, JsonOptions); }
        catch { return default; }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(value, JsonOptions);
        await Db.StringSetAsync(key, json);
        if (ttl > TimeSpan.Zero) await Db.KeyExpireAsync(key, ttl);
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        => Db.KeyDeleteAsync(key);
}
