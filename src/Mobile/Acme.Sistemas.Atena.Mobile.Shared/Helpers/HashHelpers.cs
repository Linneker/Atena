using System.Security.Cryptography;
using System.Text;

namespace Acme.Sistemas.Atena.Mobile.Shared.Helpers;

/// <summary>
/// Hash SHA-256 hex (lowercase) usado tanto pelo app (gerar hashBatida) quanto pelos
/// testes do servidor (verificar). Implementação igual à do MarcacaoPontoIntegridade
/// no backend (W2).
/// </summary>
public static class HashHelpers
{
    /// <summary>
    /// Calcula hashBatida que será enviado ao backend junto da batida mobile.
    /// Backend valida que o hash bate com os campos recebidos.
    /// </summary>
    public static string CalcularHashBatida(
        string funcionarioId,
        DateTime timestampLocal,
        string? tipo,
        string deviceId)
    {
        var payload = string.Join("|", new[]
        {
            funcionarioId,
            timestampLocal.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
            tipo ?? string.Empty,
            deviceId,
        });
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    public static string Sha256Hex(string input)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
