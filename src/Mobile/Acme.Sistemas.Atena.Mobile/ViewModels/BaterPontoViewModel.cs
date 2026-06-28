using Acme.Sistemas.Atena.Mobile.Services;
using Acme.Sistemas.Atena.Mobile.Services.Api;
using Acme.Sistemas.Atena.Mobile.Services.Offline;
using Acme.Sistemas.Atena.Mobile.Services.Platform;
using Acme.Sistemas.Atena.Mobile.Shared.Dtos;
using Acme.Sistemas.Atena.Mobile.Shared.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Refit;

namespace Acme.Sistemas.Atena.Mobile.ViewModels;

public partial class BaterPontoViewModel : ObservableObject
{
    private readonly ICameraService _camera;
    private readonly IBiometriaService _bio;
    private readonly IGeoService _geo;
    private readonly IDeviceCapabilityHelper _capabilities;
    private readonly IConnectivityService _conn;
    private readonly IOfflineQueue _offline;
    private readonly IAtenaApi _api;
    private readonly ISecureTokenStore _tokens;
    private readonly INotificationService _notif;

    public BaterPontoViewModel(
        ICameraService camera, IBiometriaService bio, IGeoService geo,
        IDeviceCapabilityHelper capabilities, IConnectivityService conn,
        IOfflineQueue offline, IAtenaApi api, ISecureTokenStore tokens,
        INotificationService notif)
    {
        _camera = camera; _bio = bio; _geo = geo; _capabilities = capabilities;
        _conn = conn; _offline = offline; _api = api; _tokens = tokens; _notif = notif;
    }

    [ObservableProperty] private bool _enviando;
    [ObservableProperty] private string? _resultadoMensagem;
    [ObservableProperty] private string? _resultadoHash;

    [RelayCommand]
    private async Task BaterAsync()
    {
        Enviando = true;
        ResultadoMensagem = null;
        ResultadoHash = null;
        try
        {
            var deviceId = await _tokens.GetAsync("atena.deviceId") ?? Guid.NewGuid().ToString("N");
            var funcionarioId = await _tokens.GetAsync("atena.funcionarioId") ?? "00000000-0000-0000-0000-000000000000";
            var capabilities = await _capabilities.InspecionarAsync();

            // Caminho da prova
            byte[]? fotoBytes = null;
            string? provaBio = null;

            if (capabilities.TemCamera)
            {
                fotoBytes = await _camera.CapturarFotoAsync();
                if (fotoBytes is null)
                {
                    // usuário cancelou. Tenta biometria como fallback.
                    if (capabilities.TemBiometria)
                        provaBio = await _bio.AutenticarEEmitirProvaAsync("Bater ponto");
                    if (string.IsNullOrEmpty(provaBio))
                    {
                        ResultadoMensagem = "Cancelado.";
                        return;
                    }
                }
            }
            else if (capabilities.TemBiometria)
            {
                provaBio = await _bio.AutenticarEEmitirProvaAsync("Bater ponto");
                if (string.IsNullOrEmpty(provaBio))
                {
                    ResultadoMensagem = "Biometria obrigatória neste dispositivo (sem câmera).";
                    return;
                }
            }
            else
            {
                ResultadoMensagem = "Dispositivo sem câmera nem biometria — não é possível bater ponto.";
                return;
            }

            var gps = await _geo.ObterCoordenadaAtualAsync();
            var timestamp = DateTime.UtcNow;
            var hash = HashHelpers.CalcularHashBatida(funcionarioId, timestamp, null, deviceId);

            var form = new BaterPontoMobileForm(
                Tipo: null,
                Latitude: gps?.Latitude,
                Longitude: gps?.Longitude,
                DeviceId: deviceId,
                TimestampLocal: timestamp,
                HashBatida: hash,
                ProvaBiometriaLocal: provaBio);

            if (!_conn.EstaOnline)
            {
                await _offline.EnfileirarBatidaAsync(form, fotoBytes);
                ResultadoMensagem = "⚠ Offline — batida salva. Sincronizará automaticamente quando voltar à rede.";
                return;
            }

            try
            {
                StreamPart fotoPart;
                if (fotoBytes is not null)
                {
                    fotoPart = new StreamPart(new MemoryStream(fotoBytes), "batida.jpg", "image/jpeg");
                }
                else
                {
                    fotoPart = new StreamPart(new MemoryStream(0), "no-photo.jpg", "image/jpeg");
                }

                var resp = await _api.BaterPontoMobileAsync(
                    fotoPart, null, form.Latitude, form.Longitude,
                    form.DeviceId, form.TimestampLocal, form.HashBatida, form.ProvaBiometriaLocal);

                ResultadoMensagem = $"✓ {resp.Tipo} registrada às {resp.DataHora.ToLocalTime():HH:mm:ss}";
                ResultadoHash = resp.HashIntegridade[..16] + "…";
            }
            catch (ApiException ex)
            {
                // Backend rejeitou → salva offline para tentar depois OU notifica
                await _offline.EnfileirarBatidaAsync(form, fotoBytes);
                ResultadoMensagem = $"Erro ({(int)ex.StatusCode}) — salva na fila para nova tentativa.";
            }
        }
        finally
        {
            Enviando = false;
        }
    }
}
