using Acme.Sistemas.Atena.Mobile.Services.Api;
using Acme.Sistemas.Atena.Mobile.Services.Platform;
using Acme.Sistemas.Atena.Mobile.Shared.Dtos;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Acme.Sistemas.Atena.Mobile.ViewModels;

public partial class PrimeiroAcessoViewModel : ObservableObject
{
    private readonly IDeviceCapabilityHelper _capabilities;
    private readonly IAtenaApi _api;
    private readonly Services.ISecureTokenStore _tokens;

    public PrimeiroAcessoViewModel(IDeviceCapabilityHelper capabilities, IAtenaApi api, Services.ISecureTokenStore tokens)
    {
        _capabilities = capabilities;
        _api = api;
        _tokens = tokens;
    }

    [ObservableProperty] private bool _testandoCamera;
    [ObservableProperty] private bool _testandoBio;
    [ObservableProperty] private string? _statusCamera;
    [ObservableProperty] private string? _statusBio;

    [RelayCommand]
    private async Task RegistrarDispositivoAsync()
    {
        var caps = await _capabilities.InspecionarAsync();
        StatusCamera = caps.TemCamera ? "Câmera disponível ✓" : "Sem câmera — biometria obrigatória";
        StatusBio = caps.TemBiometria ? "Biometria disponível ✓" : "Sem biometria";

        var deviceId = await _tokens.GetAsync("atena.deviceId") ?? Guid.NewGuid().ToString("N");
        await _tokens.SaveAsync("atena.deviceId", deviceId);

        var plataforma = caps.Plataforma switch
        {
            "Android" => PlataformaMobileDto.Android,
            "iOS" => PlataformaMobileDto.iOS,
            "MacCatalyst" or "Mac" => PlataformaMobileDto.MacOS,
            _ => PlataformaMobileDto.Windows,
        };

        try
        {
            await _api.RegistrarDispositivoAsync(new RegistrarDispositivoRequest(
                DeviceId: deviceId,
                Plataforma: plataforma,
                Modelo: caps.Modelo,
                OsVersion: caps.OsVersion,
                AppVersion: AppInfo.Current.VersionString,
                PushToken: null,
                ChavePublicaLocal: null));
            await Shell.Current.GoToAsync("//home");
        }
        catch (Refit.ApiException ex)
        {
            await Application.Current!.Windows[0].Page!.DisplayAlert(
                "Erro", $"Falha ao registrar dispositivo: {ex.Message}", "OK");
        }
    }
}
