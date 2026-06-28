using Acme.Sistemas.Atena.Mobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Acme.Sistemas.Atena.Mobile.ViewModels;

public partial class HomeViewModel : ObservableObject
{
    private readonly IConnectivityService _conn;
    private readonly System.Timers.Timer _relogio;

    public HomeViewModel(IConnectivityService conn)
    {
        _conn = conn;
        _conn.StatusMudou += (_, online) => MainThread.BeginInvokeOnMainThread(() => Online = online);
        Online = _conn.EstaOnline;

        _relogio = new System.Timers.Timer(1000);
        _relogio.Elapsed += (_, _) => MainThread.BeginInvokeOnMainThread(() => HoraAtual = DateTime.Now.ToString("HH:mm:ss"));
        _relogio.Start();
    }

    [ObservableProperty] private string _horaAtual = DateTime.Now.ToString("HH:mm:ss");
    [ObservableProperty] private bool _online;

    [RelayCommand]
    private async Task IrParaBaterPontoAsync() => await Shell.Current.GoToAsync("bater-ponto");
}
