using System.Text.Json;

namespace Acme.Sistemas.Domain.Interfaces.AppConfiguration;

public enum FeatureFlagType { String, Boolean, Integer, Double }

public sealed record FeatureFlagItem(string Key, object? Value, FeatureFlagType Type);

public interface IFeatureFlagService
{
    IReadOnlyList<FeatureFlagItem> ListAll();
    FeatureFlagItem? Get(string key);
    Task SetAsync(string key, JsonElement value, CancellationToken cancellationToken = default);
    Task<DateTime> ReloadAsync(CancellationToken cancellationToken = default);
}
