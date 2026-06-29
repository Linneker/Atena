using System.Security.Cryptography.X509Certificates;

namespace Acme.Sistemas.Domain.Interfaces.Rh;

/// <summary>
/// Assina o payload texto do comprovante de marcação (Portaria 671/2021 anexo II)
/// usando o certificado ICP-Brasil A1/A3 do empregador. RSA-SHA-256 (PKCS#1 v1.5),
/// retornando assinatura em Base64.
/// </summary>
public interface IAssinadorComprovante671
{
    AssinaturaComprovante671 Assinar(string payloadTexto, X509Certificate2 cert);
}

public sealed record AssinaturaComprovante671(
    string AssinaturaBase64,
    string HashSha256Hex,
    string CertificadoThumbprint);
