using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Aej;

/// <summary>
/// Assinatura JWS (RFC 7515) DETACHED do AEJ: header.JSON.signature, onde a parte do
/// payload é omitida (cliente verifica usando o arquivo AEJ separado). Algoritmo RS256.
/// </summary>
public sealed class AssinadorAej
{
    public string AssinarDetached(byte[] payloadJsonUtf8, X509Certificate2 cert)
    {
        if (!cert.HasPrivateKey)
            throw new InvalidOperationException("Cert sem chave privada — impossível assinar AEJ.");

        var header = new
        {
            alg = "RS256",
            typ = "JWT",
            x5c = new[] { Convert.ToBase64String(cert.RawData) },
            b64 = false,                              // sinaliza payload externo
            crit = new[] { "b64" },
        };
        var headerJson = JsonSerializer.SerializeToUtf8Bytes(header,
            new JsonSerializerOptions { DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull });

        var encodedHeader = Base64UrlEncode(headerJson);
        var signingInput = Encoding.ASCII.GetBytes(encodedHeader + ".")
            .Concat(payloadJsonUtf8).ToArray();

        using var rsa = cert.GetRSAPrivateKey()
            ?? throw new InvalidOperationException("Cert sem chave RSA.");
        var sig = rsa.SignData(signingInput, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        var encodedSig = Base64UrlEncode(sig);

        // JWS detached: header..signature  (payload vazio entre os dois pontos)
        return $"{encodedHeader}..{encodedSig}";
    }

    private static string Base64UrlEncode(byte[] bytes) =>
        Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
}
