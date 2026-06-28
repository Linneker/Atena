using Acme.Sistemas.Atena.Mobile.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Acme.Sistemas.Atena.Mobile.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly IAuthService _auth;
    private readonly INotificationService _notif;

    public LoginViewModel(IAuthService auth, INotificationService notif)
    {
        _auth = auth;
        _notif = notif;
    }

    [ObservableProperty] private string _email = string.Empty;
    [ObservableProperty] private string _senha = string.Empty;
    [ObservableProperty] private bool _carregando;
    [ObservableProperty] private string? _erro;

    [RelayCommand]
    private async Task EntrarAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Senha))
        {
            Erro = "Informe e-mail e senha.";
            return;
        }
        Carregando = true;
        Erro = null;
        try
        {
            var r = await _auth.LoginAsync(Email.Trim(), Senha);
            if (r is null)
            {
                Erro = "Credenciais inválidas ou conta bloqueada.";
                return;
            }
            await Shell.Current.GoToAsync("//home");
        }
        finally
        {
            Carregando = false;
        }
    }
}
