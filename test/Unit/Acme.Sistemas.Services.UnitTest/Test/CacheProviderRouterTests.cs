using Acme.Sistemas.Core.Settings;
using Acme.Sistemas.Infrastructure.Cache;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using StackExchange.Redis;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class CacheProviderRouterTests : IDisposable
{
    private readonly string _file;
    private readonly LiteDbCacheStore _cold;
    private readonly IMemoryCache _memory;
    private readonly HybridCacheStore _hybrid;

    public CacheProviderRouterTests()
    {
        _file = Path.Combine(Path.GetTempPath(), $"atena-router-{Guid.NewGuid():N}.db");
        _cold = new LiteDbCacheStore(_file);
        _memory = new MemoryCache(new MemoryCacheOptions());
        _hybrid = new HybridCacheStore(_memory, _cold);
    }

    public void Dispose()
    {
        _memory.Dispose();
        _cold.Dispose();
        if (File.Exists(_file)) try { File.Delete(_file); } catch { }
    }

    private sealed class StaticMonitor<T> : IOptionsMonitor<T>
    {
        public StaticMonitor(T value) { CurrentValue = value; }
        public T CurrentValue { get; private set; }
        public T Get(string? name) => CurrentValue;
        public IDisposable? OnChange(Action<T, string?> listener) => null;
        public void Update(T value) => CurrentValue = value;
    }

    [Fact]
    public async Task ProviderLiteDb_UsaHybrid()
    {
        var monitor = new StaticMonitor<FeatureFlagSettings>(new FeatureFlagSettings
        {
            Cache = new CacheFlags { Provider = "LiteDb" }
        });
        var router = new CacheProviderRouter(_hybrid, monitor,
            NullLogger<CacheProviderRouter>.Instance, redis: null);

        await router.SetAsync("k", "v", TimeSpan.FromMinutes(5));
        (await router.GetAsync<string>("k")).Should().Be("v");
        (await _cold.GetAsync<string>("k")).Should().Be("v");
    }

    [Fact]
    public async Task ProviderRedis_SemRedisRegistrado_CaiNoHybrid()
    {
        var monitor = new StaticMonitor<FeatureFlagSettings>(new FeatureFlagSettings
        {
            Cache = new CacheFlags { Provider = "Redis" }
        });
        var router = new CacheProviderRouter(_hybrid, monitor,
            NullLogger<CacheProviderRouter>.Instance, redis: null);

        await router.SetAsync("k", "v", TimeSpan.FromMinutes(5));
        (await router.GetAsync<string>("k")).Should().Be("v");
    }

    [Fact]
    public async Task HotSwap_AlteraProviderEmRuntime_RedisFalhando_CaiNoHybrid()
    {
        var monitor = new StaticMonitor<FeatureFlagSettings>(new FeatureFlagSettings
        {
            Cache = new CacheFlags { Provider = "LiteDb" }
        });

        var muxMock = new Mock<IConnectionMultiplexer>();
        muxMock.Setup(m => m.GetDatabase(It.IsAny<int>(), It.IsAny<object?>()))
               .Throws(new RedisConnectionException(ConnectionFailureType.UnableToConnect, "stub"));
        var failingRedis = new RedisCacheStore(muxMock.Object);

        var router = new CacheProviderRouter(_hybrid, monitor,
            NullLogger<CacheProviderRouter>.Instance, failingRedis);

        await router.SetAsync("k1", "v1", TimeSpan.FromMinutes(5));
        (await router.GetAsync<string>("k1")).Should().Be("v1");

        // Swap para Redis: como Redis "falha", router cai para Hybrid e ainda funciona.
        monitor.Update(new FeatureFlagSettings { Cache = new CacheFlags { Provider = "Redis" } });
        await router.SetAsync("k2", "v2", TimeSpan.FromMinutes(5));
        (await router.GetAsync<string>("k2")).Should().Be("v2");
    }

}
