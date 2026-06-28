using Acme.Sistemas.Atena.Mobile.Services;
using Acme.Sistemas.Atena.Mobile.Services.Api;
using Acme.Sistemas.Atena.Mobile.Shared.Dtos;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Acme.Sistemas.Atena.Mobile.ViewModels;

public partial class ConfiguracoesViewModel : ObservableObject
{
    private readonly IAuthService _auth;
    private readonly IAtenaApi _api;
    private readonly INotificationService _notif;

    public ConfiguracoesViewModel(IAuthService auth, IAtenaApi api, INotificationService notif)
    {
        _auth = auth;
        _api = api;
        _notif = notif;
    }

    [ObservableProperty] private ConfiguracaoMobileResponse? _config;
    [ObservableProperty] private string _appVersion = AppInfo.Current.VersionString;
    [ObservableProperty] private string _osVersion = DeviceInfo.Current.VersionString;
    [ObservableProperty] private string _plataforma = DeviceInfo.Current.Platform.ToString();
    [ObservableProperty] private string _modelo = DeviceInfo.Current.Model;

    [RelayCommand]
    private async Task CarregarConfigAsync()
    {
        try { Config = await _api.ObterConfiguracaoAsync(); }
        catch { /* silencioso — banner aparece se backend offline */ }
    }

    [RelayCommand]
    private async Task LogoutAsync()
    {
        if (!await _notif.ConfirmarAsync("Sair", "Deseja realmente sair?", "Sair", "Cancelar")) return;
        await _auth.LogoutAsync();
        await Shell.Current.GoToAsync("//login");
    }
}
