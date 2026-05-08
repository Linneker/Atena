using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;
using Acme.Sistemas.ExternalIntegration.Sefaz.Certificado;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class XmlSignerC14NTests
{
    private static X509Certificate2 GerarCertAutoAssinado()
    {
        using var rsa = RSA.Create(2048);
        var req = new CertificateRequest("CN=ATENA TESTE SIGNER", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        req.CertificateExtensions.Add(new X509KeyUsageExtension(
            X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.NonRepudiation, critical: false));
        var temp = req.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddYears(1));
        // Re-importa via PFX para garantir chave persistível em todos OS.
        var pfx = temp.Export(X509ContentType.Pfx, "x");
        return X509CertificateLoader.LoadPkcs12(pfx, "x", X509KeyStorageFlags.Exportable);
    }

    private const string XmlSample = """
        <?xml version="1.0" encoding="UTF-8"?>
        <NFe xmlns="http://www.portalfiscal.inf.br/nfe">
          <infNFe Id="NFe35260512345678000199550010000000007111908850" versao="4.00">
            <ide>
              <cUF>35</cUF>
              <natOp>VENDA</natOp>
              <mod>55</mod>
              <serie>1</serie>
              <nNF>7</nNF>
            </ide>
          </infNFe>
        </NFe>
        """;

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "XmlSignerC14N")]
    [Fact(DisplayName = "Dado um XML NFe e cert válido, quando Sign, então adiciona <Signature> filho de <NFe> com Reference URI apontando ao Id do infNFe")]
    public void Sign_AdicionaSignatureComReferenceCorreta()
    {
        var sut = new XmlSignerC14N();
        using var cert = GerarCertAutoAssinado();

        var assinado = sut.Sign(XmlSample, "NFe35260512345678000199550010000000007111908850", cert);

        var doc = new XmlDocument();
        doc.LoadXml(assinado);
        var ns = new XmlNamespaceManager(doc.NameTable);
        ns.AddNamespace("nfe", "http://www.portalfiscal.inf.br/nfe");
        ns.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

        var sig = doc.SelectSingleNode("/nfe:NFe/ds:Signature", ns);
        sig.Should().NotBeNull("Signature deve ser inserida como filho direto de <NFe>");

        var reference = doc.SelectSingleNode("/nfe:NFe/ds:Signature/ds:SignedInfo/ds:Reference", ns) as XmlElement;
        reference.Should().NotBeNull();
        reference!.GetAttribute("URI").Should().Be("#NFe35260512345678000199550010000000007111908850");

        var canonMethod = doc.SelectSingleNode("/nfe:NFe/ds:Signature/ds:SignedInfo/ds:CanonicalizationMethod", ns) as XmlElement;
        canonMethod.Should().NotBeNull();
        canonMethod!.GetAttribute("Algorithm").Should().Be(SignedXml.XmlDsigExcC14NTransformUrl);

        var sigMethod = doc.SelectSingleNode("/nfe:NFe/ds:Signature/ds:SignedInfo/ds:SignatureMethod", ns) as XmlElement;
        sigMethod.Should().NotBeNull();
        sigMethod!.GetAttribute("Algorithm").Should().Be(SignedXml.XmlDsigRSASHA1Url);

        var x509 = doc.SelectSingleNode("/nfe:NFe/ds:Signature/ds:KeyInfo/ds:X509Data/ds:X509Certificate", ns);
        x509.Should().NotBeNull("KeyInfo deve embutir o cert via X509Certificate");
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "XmlSignerC14N")]
    [Fact(DisplayName = "Dado um XML assinado pelo signer, quando re-validado por SignedXml.CheckSignature, então a assinatura é íntegra")]
    public void Sign_AssinaturaResultanteEhVerificavelPorCheckSignature()
    {
        var sut = new XmlSignerC14N();
        using var cert = GerarCertAutoAssinado();

        var assinado = sut.Sign(XmlSample, "NFe35260512345678000199550010000000007111908850", cert);

        var doc = new XmlDocument { PreserveWhitespace = true };
        doc.LoadXml(assinado);
        var sigEl = doc.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#")[0] as XmlElement;
        sigEl.Should().NotBeNull();

        var signedXml = new IdAwareSignedXmlForVerification(doc);
        signedXml.LoadXml(sigEl!);

        signedXml.CheckSignatureReturningKey(out _).Should().BeTrue(
            because: "se o próprio framework não consegue verificar, a SEFAZ vai rejeitar com erro de digesto");
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "XmlSignerC14N")]
    [Fact(DisplayName = "Dado um Id que não existe no XML, quando Sign, então lança InvalidOperationException com mensagem citando o Id")]
    public void Sign_IdInexistente_Lanca()
    {
        var sut = new XmlSignerC14N();
        using var cert = GerarCertAutoAssinado();

        Action act = () => sut.Sign(XmlSample, "NFeInexistente", cert);

        act.Should().Throw<InvalidOperationException>().WithMessage("*NFeInexistente*");
    }

    /// <summary>
    /// Subclasse que ensina o validador a achar elementos por atributo "Id" customizado.
    /// </summary>
    private sealed class IdAwareSignedXmlForVerification : SignedXml
    {
        public IdAwareSignedXmlForVerification(XmlDocument doc) : base(doc) { }

        public override XmlElement? GetIdElement(XmlDocument? document, string idValue)
        {
            if (document is null) return null;
            foreach (XmlElement el in document.GetElementsByTagName("*"))
            {
                if (el.GetAttribute("Id") == idValue) return el;
            }
            return base.GetIdElement(document, idValue);
        }
    }
}
