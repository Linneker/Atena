namespace Acme.Sistemas.Services.V1.Fiscal.Services;

public interface INFeXmlSigner
{
    /// <summary>
    /// Assina o XML NF-e usando o certificado A1 (PFX) decifrado do tenant.
    /// Implementação atual: stub que insere bloco Signature simulado.
    /// </summary>
    string Sign(string xml, byte[] pfxBytes, string pfxPassword);
}
