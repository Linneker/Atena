using Acme.Sistemas.Atena.Mobile.Services;

namespace Acme.Sistemas.Atena.Mobile;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var shell = new AppShell(_serviceProvider);
        _ = VerificarVersaoMinimaAsync();
        return new Window(shell);
    }

    protected override void OnResume()
    {
        base.OnResume();
        var offline = _serviceProvider.GetService<Services.Offline.IOfflineQueue>();
        _ = offline?.SyncPendentesAsync();
        _ = VerificarVersaoMinimaAsync();
    }

    private async Task VerificarVersaoMinimaAsync()
    {
        try
        {
            var api = _serviceProvider.GetService<Services.Api.IAtenaApi>();
            if (api is null) return;
            var cfg = await api.ObterConfiguracaoAsync();
            var atual = AppInfo.Current.VersionString;
            if (cfg.Versao.ObrigatorioAtualizar &&
                Version.TryParse(atual, out var v) &&
                Version.TryParse(cfg.Versao.MinimoSuportado, out var min) &&
                v < min)
            {
                var page = Application.Current?.Windows[0]?.Page;
                if (page is not null)
                    await page.DisplayAlert("Atualização obrigatória",
                        $"Esta versão do Atena ({atual}) não é mais suportada. " +
                        $"Atualize para {cfg.Versao.Atual}.",
                        "OK");
            }
        }
        catch { /* check de versão é best-effort */ }
    }
}
