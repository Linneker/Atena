using System.Text.Json;
using System.Text.Json.Nodes;
using Acme.Sistemas.Core.Settings;
using Acme.Sistemas.Domain.Interfaces.AppConfiguration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Acme.Sistemas.Infrastructure.AppConfiguration;

public sealed class FeatureFlagService : IFeatureFlagService
{
    private const string Root = FeatureFlagSettings.SectionName;
    private static readonly JsonSerializerOptions WriteOptions = new() { WriteIndented = true };

    private readonly IConfigurationRoot _config;
    private readonly ILogger<FeatureFlagService> _logger;
    private readonly string _filePath;
    private readonly SemaphoreSlim _gate = new(1, 1);

    public FeatureFlagService(IConfiguration config, ILogger<FeatureFlagService> logger, string filePath)
    {
        _config = (IConfigurationRoot)config;
        _logger = logger;
        _filePath = filePath;
    }

    public IReadOnlyList<FeatureFlagItem> ListAll()
    {
        var section = _config.GetSection(Root);
        var items = new List<FeatureFlagItem>();
        Walk(section, prefix: string.Empty, items);
        return items;
    }

    public FeatureFlagItem? Get(string key)
    {
        var path = $"{Root}:{key}";
        var section = _config.GetSection(path);
        if (!section.Exists() || section.Value is null && !section.GetChildren().Any())
            return null;

        if (section.Value is null) return null; // pasta intermediária, não folha
        return new FeatureFlagItem(key, ParseValue(section.Value), InferType(section.Value));
    }

    public async Task SetAsync(string key, JsonElement value, CancellationToken cancellationToken = default)
    {
        await _gate.WaitAsync(cancellationToken);
        try
        {
            // Validação de tipo: a flag precisa existir e o tipo do valor deve ser compatível.
            var current = Get(key);
            if (current is null)
                throw new ArgumentException($"Feature flag '{key}' não existe.", nameof(key));

            var coerced = CoerceOrThrow(value, current.Type, key);

            JsonNode root;
            if (File.Exists(_filePath))
            {
                using var fs = File.OpenRead(_filePath);
                root = JsonNode.Parse(fs) ?? new JsonObject();
            }
            else
            {
                root = new JsonObject();
            }

            var rootObj = root.AsObject();
            if (!rootObj.TryGetPropertyValue(Root, out var ffNode) || ffNode is not JsonObject)
            {
                ffNode = new JsonObject();
                rootObj[Root] = ffNode;
            }

            SetByPath((JsonObject)ffNode!, key.Split(':'), coerced);

            var json = root.ToJsonString(WriteOptions);
            await File.WriteAllTextAsync(_filePath, json, cancellationToken);

            // Não chama Reload — IConfiguration's reloadOnChange=true detecta o write em até ~250ms.
        }
        finally
        {
            _gate.Release();
        }
    }

    public Task<DateTime> ReloadAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _config.Reload();
            return Task.FromResult(DateTime.UtcNow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Falha ao recarregar featureflags.json — mantendo valores anteriores.");
            throw;
        }
    }

    private static void Walk(IConfigurationSection section, string prefix, List<FeatureFlagItem> items)
    {
        foreach (var child in section.GetChildren())
        {
            var key = string.IsNullOrEmpty(prefix) ? child.Key : $"{prefix}:{child.Key}";
            var hasChildren = child.GetChildren().Any();
            if (hasChildren)
            {
                Walk(child, key, items);
            }
            else
            {
                items.Add(new FeatureFlagItem(key, ParseValue(child.Value), InferType(child.Value)));
            }
        }
    }

    private static object? ParseValue(string? raw)
    {
        if (raw is null) return null;
        if (bool.TryParse(raw, out var b)) return b;
        if (long.TryParse(raw, out var i)) return i;
        if (double.TryParse(raw, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out var d)) return d;
        return raw;
    }

    private static FeatureFlagType InferType(string? raw)
    {
        if (raw is null) return FeatureFlagType.String;
        if (bool.TryParse(raw, out _)) return FeatureFlagType.Boolean;
        if (long.TryParse(raw, out _)) return FeatureFlagType.Integer;
        if (double.TryParse(raw, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out _)) return FeatureFlagType.Double;
        return FeatureFlagType.String;
    }

    private static JsonNode? CoerceOrThrow(JsonElement value, FeatureFlagType expected, string key)
    {
        switch (expected)
        {
            case FeatureFlagType.Boolean:
                if (value.ValueKind != JsonValueKind.True && value.ValueKind != JsonValueKind.False)
                    throw new ArgumentException($"Flag '{key}' espera bool; recebido {value.ValueKind}.", nameof(value));
                return JsonValue.Create(value.GetBoolean());

            case FeatureFlagType.Integer:
                if (value.ValueKind != JsonValueKind.Number || !value.TryGetInt64(out var i))
                    throw new ArgumentException($"Flag '{key}' espera inteiro; recebido {value.ValueKind}.", nameof(value));
                return JsonValue.Create(i);

            case FeatureFlagType.Double:
                if (value.ValueKind != JsonValueKind.Number)
                    throw new ArgumentException($"Flag '{key}' espera double; recebido {value.ValueKind}.", nameof(value));
                return JsonValue.Create(value.GetDouble());

            case FeatureFlagType.String:
            default:
                if (value.ValueKind == JsonValueKind.Null) return JsonValue.Create((string?)null);
                if (value.ValueKind != JsonValueKind.String)
                    throw new ArgumentException($"Flag '{key}' espera string; recebido {value.ValueKind}.", nameof(value));
                return JsonValue.Create(value.GetString());
        }
    }

    private static void SetByPath(JsonObject node, string[] path, JsonNode? value)
    {
        var current = node;
        for (var i = 0; i < path.Length - 1; i++)
        {
            if (!current.TryGetPropertyValue(path[i], out var child) || child is not JsonObject childObj)
            {
                childObj = new JsonObject();
                current[path[i]] = childObj;
            }
            current = childObj;
        }
        current[path[^1]] = value;
    }
}
