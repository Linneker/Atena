using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Domain.Interfaces.Cache;
using Microsoft.Extensions.Logging;

namespace Acme.Sistemas.Core.Mediators.Behaviors;

public sealed class CacheLookupBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ICacheStore _cache;
    private readonly ILogger<CacheLookupBehavior<TRequest, TResponse>> _logger;

    public CacheLookupBehavior(
        ICacheStore cache,
        ILogger<CacheLookupBehavior<TRequest, TResponse>> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not ICacheable cacheable) return await next();

        var key = cacheable.CacheKey;
        var cached = await _cache.GetAsync<TResponse>(key, cancellationToken);
        if (cached is not null)
        {
            _logger.LogDebug("Cache HIT {Key} para {Request}", key, typeof(TRequest).Name);
            return cached;
        }

        _logger.LogDebug("Cache MISS {Key} para {Request}", key, typeof(TRequest).Name);
        var response = await next();
        if (response is not null)
        {
            await _cache.SetAsync(key, response, cacheable.Ttl, cancellationToken);
        }
        return response;
    }
}
