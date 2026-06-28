using System.Text.Json;
using Acme.Sistemas.Atena.Mobile.Services.Api;
using Acme.Sistemas.Atena.Mobile.Shared.Dtos;
using Refit;
using SQLite;

namespace Acme.Sistemas.Atena.Mobile.Services.Offline;

/// <summary>
/// Fila local SQLite para batidas feitas offline. Sync dispara em:
///   1. App.OnResume (em MauiProgram)
///   2. Connectivity.ConnectivityChanged → Connected
///   3. Timer em background (15min WiFi / 30min cell — TODO em iteração 2)
///
/// Conflitos do servidor (HTTP 409 etc) marcam status=Failed; notifica usuário
/// para resolver via web ou solicitar ajuste.
/// </summary>
public sealed class SqliteOfflineQueue : IOfflineQueue
{
    private readonly IAtenaApi _api;
    private readonly Lazy<SQLiteAsyncConnection> _db;

    public SqliteOfflineQueue(IAtenaApi api)
    {
        _api = api;
        _db = new Lazy<SQLiteAsyncConnection>(() =>
        {
            var path = Path.Combine(FileSystem.AppDataDirectory, "atena-offline.db");
            var conn = new SQLiteAsyncConnection(path);
            _ = conn.CreateTableAsync<PendingBatida>();
            return conn;
        });
    }

    public async Task EnfileirarBatidaAsync(BaterPontoMobileForm form, byte[]? fotoBytes)
    {
        string? fotoPath = null;
        if (fotoBytes is { Length: > 0 })
        {
            var fname = $"batida-{Guid.NewGuid():N}.jpg";
            fotoPath = Path.Combine(FileSystem.AppDataDirectory, fname);
            await File.WriteAllBytesAsync(fotoPath, fotoBytes);
        }

        var entity = new PendingBatida
        {
            Id = Guid.NewGuid().ToString("N"),
            PayloadJson = JsonSerializer.Serialize(form),
            FotoPath = fotoPath,
            CriadoEm = DateTime.UtcNow,
            Status = "Pending",
            Tentativas = 0,
        };
        await _db.Value.InsertAsync(entity);
    }

    public async Task<int> SyncPendentesAsync()
    {
        var pendentes = await _db.Value.Table<PendingBatida>()
            .Where(p => p.Status == "Pending")
            .OrderBy(p => p.CriadoEm)
            .ToListAsync();

        var sincronizadas = 0;
        foreach (var p in pendentes)
        {
            try
            {
                var form = JsonSerializer.Deserialize<BaterPontoMobileForm>(p.PayloadJson)!;
                StreamPart? fotoPart = null;
                FileStream? fs = null;
                if (!string.IsNullOrEmpty(p.FotoPath) && File.Exists(p.FotoPath))
                {
                    fs = File.OpenRead(p.FotoPath);
                    fotoPart = new StreamPart(fs, Path.GetFileName(p.FotoPath), "image/jpeg");
                }

                if (fotoPart is null)
                {
                    // bater-mobile exige foto OU provaBio — se nenhum dos dois, marca falha local
                    if (string.IsNullOrEmpty(form.ProvaBiometriaLocal))
                    {
                        p.Status = "Failed";
                        p.Tentativas++;
                        await _db.Value.UpdateAsync(p);
                        continue;
                    }
                    // Sem foto mas com prova biométrica — envia stream vazio
                    fotoPart = new StreamPart(new MemoryStream(0), "no-photo.jpg", "image/jpeg");
                }

                await _api.BaterPontoMobileAsync(
                    fotoPart!,
                    form.Tipo?.ToString(),
                    form.Latitude, form.Longitude,
                    form.DeviceId, form.TimestampLocal, form.HashBatida,
                    form.ProvaBiometriaLocal);

                p.Status = "Synced";
                p.SincronizadoEm = DateTime.UtcNow;
                await _db.Value.UpdateAsync(p);
                fs?.Dispose();
                sincronizadas++;
            }
            catch (Exception ex)
            {
                p.Tentativas++;
                p.UltimaTentativaEm = DateTime.UtcNow;
                p.UltimoErro = ex.Message[..Math.Min(ex.Message.Length, 500)];
                if (p.Tentativas >= 5) p.Status = "Failed";
                await _db.Value.UpdateAsync(p);
            }
        }
        return sincronizadas;
    }

    public Task<int> ContagemPendentesAsync()
        => _db.Value.Table<PendingBatida>().Where(p => p.Status == "Pending").CountAsync();

    public async Task LimparAntigasAsync(int diasMin = 30)
    {
        var corte = DateTime.UtcNow.AddDays(-diasMin);
        await _db.Value.ExecuteAsync(
            "DELETE FROM PendingBatida WHERE Status = ? AND SincronizadoEm < ?",
            "Synced", corte);
    }
}

public sealed class PendingBatida
{
    [PrimaryKey] public string Id { get; set; } = string.Empty;
    public string PayloadJson { get; set; } = string.Empty;
    public string? FotoPath { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? SincronizadoEm { get; set; }
    public DateTime? UltimaTentativaEm { get; set; }
    /// <summary>Pending | Synced | Failed.</summary>
    public string Status { get; set; } = "Pending";
    public int Tentativas { get; set; }
    public string? UltimoErro { get; set; }
}
