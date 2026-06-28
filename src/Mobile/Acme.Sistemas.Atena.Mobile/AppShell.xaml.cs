using Acme.Sistemas.Atena.Mobile.Services;
using Acme.Sistemas.Atena.Mobile.Views;

namespace Acme.Sistemas.Atena.Mobile;

public partial class AppShell : Shell
{
    private readonly IServiceProvider _serviceProvider;

    public AppShell(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;

        // Rotas modais / detalhe (não no Flyout).
        Routing.RegisterRoute("login", typeof(LoginPage));
        Routing.RegisterRoute("primeiro-acesso", typeof(PrimeiroAcessoPage));
        Routing.RegisterRoute("bater-ponto", typeof(BaterPontoPage));
    }

    protected override async void OnNavigated(ShellNavigatedEventArgs args)
    {
        base.OnNavigated(args);

        // Guard de autenticação: usuário sem token → redireciona pra Login.
        if (args.Current.Location.OriginalString.Contains("login")) return;

        var tokenStore = _serviceProvider.GetRequiredService<ISecureTokenStore>();
        var token = await tokenStore.GetAccessTokenAsync();
        if (string.IsNullOrEmpty(token))
            await GoToAsync("//login");
    }
}
