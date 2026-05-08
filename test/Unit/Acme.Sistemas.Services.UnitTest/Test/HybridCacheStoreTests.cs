using Acme.Sistemas.Infrastructure.Cache;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class HybridCacheStoreTests : IDisposable
{
    private readonly string _file;
    private readonly LiteDbCacheStore _cold;
    private readonly IMemoryCache _memory;
    private readonly HybridCacheStore _sut;

    public HybridCacheStoreTests()
    {
        _file = Path.Combine(Path.GetTempPath(), $"atena-cache-{Guid.NewGuid():N}.db");
        _cold = new LiteDbCacheStore(_file);
        _memory = new MemoryCache(new MemoryCacheOptions());
        _sut = new HybridCacheStore(_memory, _cold);
    }

    public void Dispose()
    {
        _memory.Dispose();
        _cold.Dispose();
        if (File.Exists(_file)) try { File.Delete(_file); } catch { }
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "HybridCacheStore")]
    [Fact(DisplayName = "Dado SetAsync, quando grava entrada, então persiste em memory (hot) e cold (LiteDb) simultaneamente")]
    public async Task Set_GravaEmAmbasCamadas()
    {
        await _sut.SetAsync("k", "v", TimeSpan.FromMinutes(5));
        _memory.TryGetValue("k", out string? hot).Should().BeTrue();
        hot.Should().Be("v");
        (await _cold.GetAsync<string>("k")).Should().Be("v");
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "HybridCacheStore")]
    [Fact(DisplayName = "Dado hit em memory, quando GetAsync, então retorna do hot e não consulta o cold")]
    public async Task Get_HitMemory_NaoConsultaCold()
    {
        _memory.Set("k", "hot");
        var v = await _sut.GetAsync<string>("k");
        v.Should().Be("hot");
        // Cold permanece vazio.
        (await _cold.GetAsync<string>("k")).Should().BeNull();
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "HybridCacheStore")]
    [Fact(DisplayName = "Dado miss em memory e hit em cold, quando GetAsync, então retorna do cold e repopula a memory")]
    public async Task Get_MissMemory_HitCold_RepopulaMemory()
    {
        await _cold.SetAsync("k", "cold-only", TimeSpan.FromMinutes(5));
        var v = await _sut.GetAsync<string>("k");
        v.Should().Be("cold-only");
        _memory.TryGetValue("k", out string? hot).Should().BeTrue();
        hot.Should().Be("cold-only");
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "HybridCacheStore")]
    [Fact(DisplayName = "Dado miss em memory e cold, quando GetAsync, então retorna null")]
    public async Task Get_MissAmbas_RetornaNull()
    {
        var v = await _sut.GetAsync<string>("k");
        v.Should().BeNull();
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "HybridCacheStore")]
    [Fact(DisplayName = "Dado entrada em ambas camadas, quando RemoveAsync, então remove de memory e cold")]
    public async Task Remove_RemoveDeAmbas()
    {
        await _sut.SetAsync("k", "v", TimeSpan.FromMinutes(5));
        await _sut.RemoveAsync("k");
        _memory.TryGetValue("k", out _).Should().BeFalse();
        (await _cold.GetAsync<string>("k")).Should().BeNull();
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "HybridCacheStore")]
    [Fact(DisplayName = "Dado entrada com TTL expirado no cold, quando GetAsync, então retorna null")]
    public async Task TtlExpirado_NoCold_RetornaNull()
    {
        await _cold.SetAsync("k", "v", TimeSpan.FromMilliseconds(1));
        await Task.Delay(30);
        (await _sut.GetAsync<string>("k")).Should().BeNull();
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "HybridCacheStore")]
    [Fact(DisplayName = "Dado 10 threads concorrentes escrevendo, quando todas terminam, então cold contém todas as entradas sem corrupção")]
    public async Task ConcorrenciaIntraProcesso_10Threads_NaoCorrompe()
    {
        const int threadCount = 10;
        const int perThread = 100;
        var tasks = Enumerable.Range(0, threadCount).Select(t => Task.Run(async () =>
        {
            for (var i = 0; i < perThread; i++)
            {
                var key = $"k:{t}:{i}";
                await _sut.SetAsync(key, $"v:{t}:{i}", TimeSpan.FromMinutes(5));
            }
        })).ToArray();

        await Task.WhenAll(tasks);

        for (var t = 0; t < threadCount; t++)
        {
            var sample = await _sut.GetAsync<string>($"k:{t}:0");
            sample.Should().Be($"v:{t}:0");
        }

        _cold.CountEntries().Should().Be(threadCount * perThread);
    }
}
