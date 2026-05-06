using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Acme.Sistemas.Core.Settings;

namespace Acme.Sistemas.Infrastructure.AppConfiguration;

public interface IFeatureFlagService
{
    bool IsEnabled(string flagName);
    Task ReloadAsync(CancellationToken cancellationToken = default);
}

public sealed class FeatureFlagService : IFeatureFlagService
{
    private readonly FeatureFlagSettings _settings;
    private readonly ILogger<FeatureFlagService> _logger;
    private Dictionary<string, bool> _flags;

    public FeatureFlagService(IOptions<FeatureFlagSettings> options, ILogger<FeatureFlagService> logger)
    {
        _settings = options.Value;
        _logger = logger;
        _flags = new Dictionary<string, bool>(_settings.Defaults, StringComparer.OrdinalIgnoreCase);
        _ = ReloadAsync();
    }

    public bool IsEnabled(string flagName)
        => _flags.TryGetValue(flagName, out var v) && v;

    public async Task ReloadAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (!File.Exists(_settings.FilePath)) return;

            await using var fs = File.OpenRead(_settings.FilePath);
            var doc = await JsonSerializer.DeserializeAsync<FeatureFlagFile>(fs, cancellationToken: cancellationToken);
            if (doc?.Flags is not null)
            {
                _flags = new Dictionary<string, bool>(doc.Flags, StringComparer.OrdinalIgnoreCase);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Falha ao recarregar feature flags. Mantendo valores anteriores.");
        }
    }

    private sealed record FeatureFlagFile(Dictionary<string, bool> Flags);
}
