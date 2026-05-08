namespace Acme.Sistemas.Core.Settings;

public sealed class FeatureFlagSettings
{
    public const string SectionName = "FeatureFlags";

    public CacheFlags Cache { get; set; } = new();
    public NFeFlags NFe { get; set; } = new();
    public AuditFlags Audit { get; set; } = new();
}

public sealed class CacheFlags
{
    /// <summary>"LiteDb" (default) ou "Redis".</summary>
    public string Provider { get; set; } = "LiteDb";
    public int HotTtlMinutes { get; set; } = 15;
    public int ColdTtlMinutes { get; set; } = 15;
    public string LiteDbPath { get; set; } = "cache.db";
    public string? RedisConnection { get; set; }
}

public sealed class NFeFlags
{
    public bool AmbienteHomologacao { get; set; } = true;
    public bool ContingenciaSvrsAuto { get; set; } = true;
}

public sealed class AuditFlags
{
    public bool Enabled { get; set; } = true;
    public bool Verbose { get; set; } = false;
}
