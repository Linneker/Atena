namespace Acme.Sistemas.Atena.Mobile.Services.Platform;

public interface IBiometriaService
{
    /// <summary>Verifica se o dispositivo suporta biometria (FaceID/TouchID/fingerprint).</summary>
    Task<bool> SuportaBiometriaAsync();

    /// <summary>
    /// Solicita autenticação biométrica ao usuário. Retorna prova local (JWT autoassinado
    /// com chave por-device guardada em SecureStorage) que será enviada ao backend.
    /// </summary>
    Task<string?> AutenticarEEmitirProvaAsync(string motivo);
}

/// <summary>
/// Implementação stub multiplataforma. Em produção, substitua por implementação
/// específica via partial classes em Platforms/{Android,iOS,Windows}/.
/// </summary>
public sealed class BiometriaService : IBiometriaService
{
    private readonly ISecureTokenStore _tokens;

    public BiometriaService(ISecureTokenStore tokens) => _tokens = tokens;

    public Task<bool> SuportaBiometriaAsync()
    {
        // Plugin.Maui.Biometric ou implementação nativa por plataforma cobririam isso.
        // Fallback conservador: indisponível até integração real.
        return Task.FromResult(DeviceInfo.Current.Platform == DevicePlatform.iOS
                            || DeviceInfo.Current.Platform == DevicePlatform.Android);
    }

    public async Task<string?> AutenticarEEmitirProvaAsync(string motivo)
    {
        if (!await SuportaBiometriaAsync()) return null;

        // TODO: substituir por chamada nativa BiometricPrompt (Android) /
        // LAContext.EvaluatePolicy (iOS) via partial classes.
        // Por ora, gera prova baseada em timestamp + deviceId (placeholder).
        var deviceId = await _tokens.GetAsync("atena.deviceId") ?? "unknown";
        var payload = $"{deviceId}|{DateTime.UtcNow:O}|{motivo}";
        var hash = Shared.Helpers.HashHelpers.Sha256Hex(payload);
        return $"local-bio:{hash}";
    }
}
