namespace Acme.Sistemas.Core.Mediators;

public interface ICacheable
{
    string CacheKey { get; }
    TimeSpan Ttl { get; }
}
