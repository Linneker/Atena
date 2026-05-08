using Acme.Sistemas.Atena.Api.Hosted;
using Acme.Sistemas.Infrastructure.Cache;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class CacheCleanupWorkerTests : IDisposable
{
    private readonly string _file;
    private readonly LiteDbCacheStore _cold;

    public CacheCleanupWorkerTests()
    {
        _file = Path.Combine(Path.GetTempPath(), $"atena-cleanup-{Guid.NewGuid():N}.db");
        _cold = new LiteDbCacheStore(_file);
    }

    public void Dispose()
    {
        _cold.Dispose();
        if (File.Exists(_file)) try { File.Delete(_file); } catch { }
    }

    [Trait("Solucao", "Infrastructure")]
    [Trait("Acao", "CacheCleanupWorker")]
    [Fact(DisplayName = "Dado entradas com TTLs distintos, quando Tick executa, então remove apenas as expiradas e preserva as vigentes")]
    public async Task Tick_RemoveSomenteExpiradas()
    {
        await _cold.SetAsync("vivo", "v", TimeSpan.FromMinutes(5));
        await _cold.SetAsync("morto", "m", TimeSpan.FromMilliseconds(1));
        await Task.Delay(30);

        var worker = new CacheCleanupWorker(_cold, NullLogger<CacheCleanupWorker>.Instance);
        worker.Tick();

        (await _cold.GetAsync<string>("vivo")).Should().Be("v");
        (await _cold.GetAsync<string>("morto")).Should().BeNull();
    }
}
