using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Behaviors;
using Acme.Sistemas.Core.Mediators.Cache;
using Acme.Sistemas.Core.Mediators.Handler;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class CacheLookupBehaviorTests
{
    public sealed record CacheableQuery(string Id) : IRequest<string>, ICacheable
    {
        public string CacheKey => $"q:{Id}";
        public TimeSpan Ttl => TimeSpan.FromMinutes(5);
    }

    public sealed record NaoCacheableQuery(string Id) : IRequest<string>;

    [Trait("Solucao", "Services")]
    [Trait("Acao", "CacheLookupBehavior")]
    [Fact(DisplayName = "Dado um request não-ICacheable, quando o behavior executa, então ignora cache e chama o próximo")]
    public async Task RequestNaoCacheable_NaoConsultaCache_ChamaProximo()
    {
        var cache = new InMemoryCacheStore();
        var sut = new CacheLookupBehavior<NaoCacheableQuery, string>(
            cache, NullLogger<CacheLookupBehavior<NaoCacheableQuery, string>>.Instance);

        var chamouProximo = false;
        var resultado = await sut.Handle(new NaoCacheableQuery("1"), () =>
        {
            chamouProximo = true;
            return Task.FromResult("valor");
        }, default);

        resultado.Should().Be("valor");
        chamouProximo.Should().BeTrue();
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "CacheLookupBehavior")]
    [Fact(DisplayName = "Dado cache miss em request ICacheable, quando o behavior executa, então chama o próximo e grava o resultado no cache")]
    public async Task Miss_ChamaProximo_GravaCache()
    {
        var cache = new InMemoryCacheStore();
        var sut = new CacheLookupBehavior<CacheableQuery, string>(
            cache, NullLogger<CacheLookupBehavior<CacheableQuery, string>>.Instance);

        var resultado = await sut.Handle(new CacheableQuery("1"), () => Task.FromResult("valor"), default);

        resultado.Should().Be("valor");
        (await cache.GetAsync<string>("q:1")).Should().Be("valor");
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "CacheLookupBehavior")]
    [Fact(DisplayName = "Dado cache hit em request ICacheable, quando o behavior executa, então retorna valor cacheado e não chama o próximo")]
    public async Task Hit_RetornaValorCacheado_NaoChamaProximo()
    {
        var cache = new InMemoryCacheStore();
        await cache.SetAsync("q:1", "cacheado", TimeSpan.FromMinutes(5));
        var sut = new CacheLookupBehavior<CacheableQuery, string>(
            cache, NullLogger<CacheLookupBehavior<CacheableQuery, string>>.Instance);

        var chamouProximo = false;
        var resultado = await sut.Handle(new CacheableQuery("1"), () =>
        {
            chamouProximo = true;
            return Task.FromResult("novo");
        }, default);

        resultado.Should().Be("cacheado");
        chamouProximo.Should().BeFalse();
    }

    [Trait("Solucao", "Services")]
    [Trait("Acao", "CacheLookupBehavior")]
    [Fact(DisplayName = "Dado cache com TTL expirado, quando o behavior executa, então não retorna valor antigo e chama o próximo")]
    public async Task TtlExpirado_NaoRetornaValor_ChamaProximo()
    {
        var cache = new InMemoryCacheStore();
        await cache.SetAsync("q:1", "antigo", TimeSpan.FromMilliseconds(1));
        await Task.Delay(20);
        var sut = new CacheLookupBehavior<CacheableQuery, string>(
            cache, NullLogger<CacheLookupBehavior<CacheableQuery, string>>.Instance);

        var resultado = await sut.Handle(new CacheableQuery("1"), () => Task.FromResult("novo"), default);

        resultado.Should().Be("novo");
    }
}
