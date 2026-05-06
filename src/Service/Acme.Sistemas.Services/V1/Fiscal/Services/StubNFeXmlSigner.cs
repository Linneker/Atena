using System.Security.Cryptography;
using System.Text;

namespace Acme.Sistemas.Services.V1.Fiscal.Services;

/// <summary>
/// STUB: assinatura simulada para fluxo de testes.
/// Em produção, implementar XmlDsig com SignedXml + X509Certificate2 e referência canônica
/// conforme NT NFE 2018.005-v1.10.
/// </summary>
public sealed class StubNFeXmlSigner : INFeXmlSigner
{
    public string Sign(string xml, byte[] pfxBytes, string pfxPassword)
    {
        var hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(xml)));
        var fakeSignature =
            $"<Signature xmlns=\"http://www.w3.org/2000/09/xmldsig#\">" +
            $"<SignedInfo><CanonicalizationMethod Algorithm=\"http://www.w3.org/TR/2001/REC-xml-c14n-20010315\"/>" +
            $"<SignatureMethod Algorithm=\"http://www.w3.org/2000/09/xmldsig#rsa-sha1\"/>" +
            $"<Reference URI=\"\"><DigestValue>{hash}</DigestValue></Reference></SignedInfo>" +
            $"<SignatureValue>STUB-SIGNATURE-NOT-FOR-PRODUCTION</SignatureValue>" +
            $"</Signature>";

        // Insere antes de </NFe>
        var idx = xml.LastIndexOf("</NFe>", StringComparison.Ordinal);
        return idx < 0 ? xml : xml.Insert(idx, fakeSignature);
    }
}
