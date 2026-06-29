using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Acme.Sistemas.Domain.Interfaces.Rh;

namespace Acme.Sistemas.ExternalIntegration.Rh.Oficial671;

/// <summary>
/// Assina o comprovante 671 anexo II com a chave privada do certificado do empregador:
///   RSA-SHA-256 (PKCS#1 v1.5) sobre UTF-8 bytes do payloadTexto.
/// Reusa as primitivas do <c>XmlSignerC14N</c> em espírito (cert + RSA + SHA), mas o payload
/// é texto plano (não XML), então pulamos canonicalização e usamos assinatura direta.
/// </summary>
public sealed class AssinadorComprovante671 : IAssinadorComprovante671
{
    public AssinaturaComprovante671 Assinar(string payloadTexto, X509Certificate2 cert)
    {
        if (string.IsNullOrEmpty(payloadTexto))
            throw new ArgumentException("payloadTexto vazio.", nameof(payloadTexto));
        if (!cert.HasPrivateKey)
            throw new InvalidOperationException("Certificado sem chave privada — impossível assinar comprovante 671.");

        var bytes = Encoding.UTF8.GetBytes(payloadTexto);
        var hash = SHA256.HashData(bytes);

        using var rsa = cert.GetRSAPrivateKey()
            ?? throw new InvalidOperationException("Certificado ICP-Brasil sem chave RSA — apenas RSA é suportado pela Portaria 671.");
        var assinatura = rsa.SignData(bytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

        return new AssinaturaComprovante671(
            AssinaturaBase64: Convert.ToBase64String(assinatura),
            HashSha256Hex: Convert.ToHexString(hash).ToLowerInvariant(),
            CertificadoThumbprint: cert.Thumbprint);
    }
}
