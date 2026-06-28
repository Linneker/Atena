namespace Acme.Sistemas.Atena.Mobile;

/// <summary>
/// Configurações da app. Em DEBUG aponta para o backend local; em RELEASE
/// para o backend de produção. Sobrescrito por env vars/build args.
/// </summary>
public sealed class AppSettings
{
    public string ApiBaseUrl { get; init; } = "https://api.atena.local/";
    public int TimeoutSeconds { get; init; } = 30;

    public static AppSettings LoadFromEnvironment() => new()
    {
#if DEBUG
        ApiBaseUrl = Environment.GetEnvironmentVariable("ATENA_API_URL") ?? "https://10.0.2.2:5001/",
#else
        ApiBaseUrl = Environment.GetEnvironmentVariable("ATENA_API_URL") ?? "https://api.atena.com.br/",
#endif
    };
}
