namespace Acme.Sistemas.Core.Settings;

public sealed class CacheMetrics
{
    public long Hits { get; set; }
    public long Misses { get; set; }
    public long Evictions { get; set; }
    public double HitRate => Hits + Misses == 0 ? 0 : (double)Hits / (Hits + Misses);
}
