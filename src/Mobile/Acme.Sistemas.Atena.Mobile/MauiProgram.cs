using Acme.Sistemas.Atena.Mobile.Services;
using Acme.Sistemas.Atena.Mobile.Services.Api;
using Acme.Sistemas.Atena.Mobile.Services.Offline;
using Acme.Sistemas.Atena.Mobile.Services.Platform;
using Acme.Sistemas.Atena.Mobile.ViewModels;
using Acme.Sistemas.Atena.Mobile.Views;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using Refit;

namespace Acme.Sistemas.Atena.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // ---------------- Settings -----------------
        builder.Services.AddSingleton(AppSettings.LoadFromEnvironment());

        // ---------------- Core services -----------------
        builder.Services.AddSingleton<ISecureTokenStore, SecureTokenStore>();
        builder.Services.AddSingleton<IConnectivityService, ConnectivityService>();
        builder.Services.AddSingleton<IDeviceCapabilityHelper, DeviceCapabilityHelper>();
        builder.Services.AddSingleton<IBiometriaService, BiometriaService>();
        builder.Services.AddSingleton<ICameraService, CameraService>();
        builder.Services.AddSingleton<IGeoService, GeoService>();
        builder.Services.AddSingleton<IOfflineQueue, SqliteOfflineQueue>();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<INotificationService, NotificationService>();
        builder.Services.AddSingleton(WeakReferenceMessenger.Default);

        // ---------------- HTTP client + Refit ----------------
        builder.Services.AddTransient<AuthDelegatingHandler>();

        builder.Services
            .AddRefitClient<IAtenaApi>()
            .ConfigureHttpClient((sp, c) =>
            {
                var settings = sp.GetRequiredService<AppSettings>();
                c.BaseAddress = new Uri(settings.ApiBaseUrl);
                c.Timeout = TimeSpan.FromSeconds(30);
            })
            .AddHttpMessageHandler<AuthDelegatingHandler>()
            .AddPolicyHandler(GetRetryPolicy());

        // ---------------- ViewModels ----------------
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<BaterPontoViewModel>();
        builder.Services.AddTransient<EspelhoMensalViewModel>();
        builder.Services.AddTransient<AjustesPageViewModel>();
        builder.Services.AddTransient<ConfiguracoesViewModel>();
        builder.Services.AddTransient<PrimeiroAcessoViewModel>();

        // ---------------- Views (transientes) ----------------
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<BaterPontoPage>();
        builder.Services.AddTransient<EspelhoMensalPage>();
        builder.Services.AddTransient<AjustesPage>();
        builder.Services.AddTransient<ConfiguracoesPage>();
        builder.Services.AddTransient<PrimeiroAcessoPage>();

        return builder.Build();
    }

    /// <summary>3 tentativas com backoff exponencial em falhas de rede e 5xx.</summary>
    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        => HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));
}
