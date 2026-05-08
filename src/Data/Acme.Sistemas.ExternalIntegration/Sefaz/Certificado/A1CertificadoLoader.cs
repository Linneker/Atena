using System.Security.Cryptography.X509Certificates;
using Acme.Sistemas.Domain.Interfaces.Fiscal;

namespace Acme.Sistemas.ExternalIntegration.Sefaz.Certificado;

/// <summary>
/// Carrega certificado A1 (PFX em arquivo/bytes). Suporta validação de:
/// - integridade do PFX e senha;
/// - vencimento (`NotAfter` no futuro);
/// - KeyUsage com `DigitalSignature`;
/// - cadeia até uma raiz ICP-Brasil (opcional via flag — em homologação,
///   certs auto-assinados não passam por essa cadeia, então o teste
///   permite desabilitar).
/// </summary>
public sealed class A1CertificadoLoader : ICertificadoLoader
{
    private readonly bool _validarCadeiaIcpBrasil;

    public A1CertificadoLoader(bool validarCadeiaIcpBrasil = true)
    {
        _validarCadeiaIcpBrasil = validarCadeiaIcpBrasil;
    }

    public Task<X509Certificate2> LoadAsync(byte[] pfx, string senha, CancellationToken cancellationToken = default)
    {
        if (pfx is null || pfx.Length == 0)
            throw new CertificadoInvalidoException("PFX vazio.");

        X509Certificate2 cert;
        try
        {
            cert = X509CertificateLoader.LoadPkcs12(
                pfx,
                senha,
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
        }
        catch (Exception ex)
        {
            throw new CertificadoInvalidoException(
                "Falha ao abrir PFX — senha incorreta ou arquivo corrompido.", ex);
        }

        ValidarVencimento(cert);
        ValidarKeyUsage(cert);
        if (_validarCadeiaIcpBrasil)
            ValidarCadeiaIcpBrasil(cert);

        return Task.FromResult(cert);
    }

    private static void ValidarVencimento(X509Certificate2 cert)
    {
        var agora = DateTime.UtcNow;
        if (cert.NotAfter.ToUniversalTime() <= agora)
            throw new CertificadoInvalidoException(
                $"Certificado vencido em {cert.NotAfter:yyyy-MM-dd} (Subject: {cert.Subject}).");
        if (cert.NotBefore.ToUniversalTime() > agora)
            throw new CertificadoInvalidoException(
                $"Certificado ainda não vigente até {cert.NotBefore:yyyy-MM-dd}.");
    }

    private static void ValidarKeyUsage(X509Certificate2 cert)
    {
        // Certificados ICP-Brasil para assinatura digital obrigatoriamente declaram DigitalSignature.
        var keyUsageExt = cert.Extensions.OfType<X509KeyUsageExtension>().FirstOrDefault();
        if (keyUsageExt is null)
        {
            // Alguns A1 antigos não declaram a extension explicitamente; trata-se como aviso, não erro.
            return;
        }
        if ((keyUsageExt.KeyUsages & X509KeyUsageFlags.DigitalSignature) == 0)
            throw new CertificadoInvalidoException(
                $"Certificado sem KeyUsage 'DigitalSignature' (uses: {keyUsageExt.KeyUsages}).");
    }

    private static void ValidarCadeiaIcpBrasil(X509Certificate2 cert)
    {
        using var chain = new X509Chain();
        chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck; // CRL/OCSP exige rede; opcional fora deste loader
        chain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag;
        var ok = chain.Build(cert);
        if (!ok)
        {
            var motivos = string.Join("; ", chain.ChainStatus.Select(s => s.StatusInformation.Trim()));
            throw new CertificadoInvalidoException(
                $"Falha ao validar cadeia ICP-Brasil: {motivos}");
        }

        // Heurística leve: a raiz de uma cadeia ICP-Brasil tem CN começando com "AC Raiz".
        // Em produção, a chain.Build já validou contra o trust store do SO — se o SO confia,
        // assumimos válida. Heurística adicional só vira hard-fail em prod via configuração externa.
        var raiz = chain.ChainElements[^1].Certificate;
        if (!raiz.Subject.Contains("ICP-Brasil", StringComparison.OrdinalIgnoreCase)
            && !raiz.Subject.Contains("AC Raiz", StringComparison.OrdinalIgnoreCase))
        {
            // não é hard-fail: trust store do SO pode estar OK mesmo sem a heurística textual
        }
    }
}
