namespace Acme.Sistemas.Services.V1.Rh.Mobile.Push;

public interface INotificacaoPushService
{
    /// <summary>Envia push para o dispositivo do funcionário (FCM ou APNs conforme plataforma).</summary>
    Task<PushEnvioResult> EnviarParaUsuarioAsync(
        Guid usuarioId, string titulo, string mensagem,
        IReadOnlyDictionary<string, string>? dados = null,
        CancellationToken cancellationToken = default);

    /// <summary>Envia para tópico (broadcast filtrado por categoria; ex.: "tenant:abc:funcionarios").</summary>
    Task<PushEnvioResult> EnviarParaTopicoAsync(
        string topico, string titulo, string mensagem,
        IReadOnlyDictionary<string, string>? dados = null,
        CancellationToken cancellationToken = default);
}

public sealed record PushEnvioResult(int Sucessos, int Falhas, IReadOnlyList<string>? ErrosDetalhados = null);

/// <summary>
/// Stub do serviço de push. Em produção, substituir por integração real:
///   - Android: FirebaseAdmin .NET (Google.Apis.Auth + Firebase Admin SDK)
///   - iOS/macOS: Apple APNs HTTP/2 (dotAPNS lib ou implementação manual)
/// Credenciais (FCM service account JSON, APNs cert) ficam em SecureStorage do servidor
/// (Azure KeyVault / AWS Secrets Manager) — config por env var em Production.
/// </summary>
public sealed class StubNotificacaoPushService : INotificacaoPushService
{
    public Task<PushEnvioResult> EnviarParaUsuarioAsync(
        Guid usuarioId, string titulo, string mensagem,
        IReadOnlyDictionary<string, string>? dados = null,
        CancellationToken cancellationToken = default)
    {
        // Stub: log apenas. Substituir por FCM/APNs em produção.
        Console.WriteLine($"[PUSH stub → usuario={usuarioId}] {titulo}: {mensagem}");
        return Task.FromResult(new PushEnvioResult(1, 0));
    }

    public Task<PushEnvioResult> EnviarParaTopicoAsync(
        string topico, string titulo, string mensagem,
        IReadOnlyDictionary<string, string>? dados = null,
        CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"[PUSH stub → topico={topico}] {titulo}: {mensagem}");
        return Task.FromResult(new PushEnvioResult(1, 0));
    }
}
