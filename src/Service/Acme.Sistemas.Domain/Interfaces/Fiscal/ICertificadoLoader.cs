using System.Security.Cryptography.X509Certificates;

namespace Acme.Sistemas.Domain.Interfaces.Fiscal;

/// <summary>
/// Carrega certificado fiscal a partir de bytes PFX (A1) ou outras fontes (A3).
/// </summary>
public interface ICertificadoLoader
{
    /// <summary>
    /// Carrega o cert do PFX em memória. Senha em texto claro (já descriptografada pelo caller).
    /// </summary>
    /// <exception cref="CertificadoInvalidoException">Senha errada, PFX corrompido ou cadeia inválida.</exception>
    Task<X509Certificate2> LoadAsync(byte[] pfx, string senha, CancellationToken cancellationToken = default);
}

/// <summary>
/// Erro de carregamento de certificado fiscal — sempre acompanhado de motivo legível.
/// </summary>
public sealed class CertificadoInvalidoException : Exception
{
    public CertificadoInvalidoException(string message) : base(message) { }
    public CertificadoInvalidoException(string message, Exception inner) : base(message, inner) { }
}
