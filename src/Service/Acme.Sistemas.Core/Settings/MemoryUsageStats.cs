namespace Acme.Sistemas.Core.Settings;

public sealed class MemoryUsageStats
{
    public long WorkingSetBytes { get; set; }
    public long ManagedHeapBytes { get; set; }
    public int Gen0Collections { get; set; }
    public int Gen1Collections { get; set; }
    public int Gen2Collections { get; set; }
    public DateTime CollectedAt { get; set; } = DateTime.UtcNow;
}
