using System.Text.Json;
using Acme.Sistemas.Domain.Interfaces.Cache;
using LiteDB;
using Microsoft.Extensions.Logging;

namespace Acme.Sistemas.Infrastructure.Cache;

/// <summary>
/// Cold layer do cache híbrido. LiteDB single-file, single-process intra-pod.
/// Schema: coleção `cache` com documentos { _id=Key, valueJson, expiresAtTicks }.
/// `expiresAtTicks` é armazenado como long para evitar a perda de DateTimeKind do LiteDB
/// (que faz UTC voltar como Local e quebrar comparações de TTL em timezones != UTC).
/// Concorrência intra-processo é serializada por <see cref="_gate"/>.
/// </summary>
public sealed class LiteDbCacheStore : ICacheStore, IDisposable
{
    private const string CollectionName = "cache";
    private const string FieldValue = "valueJson";
    private const string FieldExpiresTicks = "expiresAtTicks";

    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    private readonly LiteDatabase _db;
    private readonly ILogger<LiteDbCacheStore>? _logger;
    private readonly object _gate = new();

    public LiteDbCacheStore(string filePath, ILogger<LiteDbCacheStore>? logger = null)
    {
        _logger = logger;
        var dir = Path.GetDirectoryName(Path.GetFullPath(filePath));
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

        var connStr = new ConnectionString(filePath) { Connection = ConnectionType.Direct };
        _db = new LiteDatabase(connStr);
        var col = _db.GetCollection(CollectionName);
        col.EnsureIndex(FieldExpiresTicks);
        FilePath = filePath;
    }

    public string FilePath { get; }

    private ILiteCollection<BsonDocument> Col => _db.GetCollection(CollectionName);

    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        BsonDocument? doc;
        lock (_gate)
        {
            doc = Col.FindById(key);
        }
        if (doc is null) return Task.FromResult(default(T?));

        var expiresAtTicks = doc[FieldExpiresTicks].AsInt64;
        if (expiresAtTicks <= DateTime.UtcNow.Ticks)
        {
            lock (_gate) Col.Delete(key);
            return Task.FromResult(default(T?));
        }
        try
        {
            var json = doc[FieldValue].AsString;
            return Task.FromResult(System.Text.Json.JsonSerializer.Deserialize<T>(json, JsonOptions));
        }
        catch (Exception ex)
        {
            _logger?.LogWarning(ex, "Falha ao desserializar cache key {Key}", key);
            return Task.FromResult(default(T?));
        }
    }

    public Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken cancellationToken = default)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(value, JsonOptions);
        var doc = new BsonDocument
        {
            ["_id"] = key,
            [FieldValue] = json,
            [FieldExpiresTicks] = DateTime.UtcNow.Add(ttl).Ticks
        };
        lock (_gate) Col.Upsert(doc);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        lock (_gate) Col.Delete(key);
        return Task.CompletedTask;
    }

    /// <summary>Remove entradas expiradas; retorna número removido.</summary>
    public int RemoveExpired()
    {
        var nowTicks = DateTime.UtcNow.Ticks;
        lock (_gate)
        {
            var expired = Col.FindAll()
                .Where(d => d[FieldExpiresTicks].AsInt64 <= nowTicks)
                .ToList();
            var removed = 0;
            foreach (var d in expired)
            {
                if (Col.Delete(d["_id"])) removed++;
            }
            return removed;
        }
    }

    /// <summary>Remove as N entradas mais antigas (por ExpiresAt crescente).</summary>
    public int RemoveOldest(int count)
    {
        if (count <= 0) return 0;
        lock (_gate)
        {
            var oldest = Col.FindAll()
                .OrderBy(d => d[FieldExpiresTicks].AsInt64)
                .Take(count)
                .ToList();
            var removed = 0;
            foreach (var entry in oldest)
            {
                if (Col.Delete(entry["_id"])) removed++;
            }
            return removed;
        }
    }

    public long EstimateSizeBytes()
    {
        try { return new FileInfo(FilePath).Length; }
        catch { return 0L; }
    }

    public long CountEntries()
    {
        lock (_gate) return Col.Count();
    }

    public void Rebuild()
    {
        lock (_gate) _db.Rebuild();
    }

    public void Dispose() => _db.Dispose();
}
