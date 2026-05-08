namespace Acme.Sistemas.Domain.Interfaces.Fiscal;

public interface INFeXmlSigner
{
    string Sign(string xml, byte[] pfxBytes, string pfxPassword);
}
