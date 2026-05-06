namespace Acme.Sistemas.Core.Settings;

public sealed class FeatureFlagSettings
{
    public const string SectionName = "FeatureFlags";

    public string FilePath { get; set; } = "featureflags.json";
    public int RefreshIntervalSeconds { get; set; } = 30;
    public Dictionary<string, bool> Defaults { get; set; } = new();
}
