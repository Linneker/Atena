using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Acme.Sistemas.ExternalIntegration.Sefaz.Certificado;

/// <summary>
/// Assinador XMLDSig para documentos NF-e (e eventos) seguindo as regras SEFAZ:
/// - Algoritmo de canonicalização: <c>http://www.w3.org/2001/10/xml-exc-c14n#</c>
/// - Algoritmo de assinatura: <c>http://www.w3.org/2000/09/xmldsig#rsa-sha1</c> (SEFAZ ainda exige SHA-1)
/// - Reference com transformações: enveloped-signature + exc-c14n
/// - URI = <c>#&lt;Id&gt;</c> apontando ao atributo `Id` do elemento sendo assinado (`infNFe`, `infEvento`, `infInut`)
/// - KeyInfo com `X509Data > X509Certificate`
///
/// O elemento `Signature` é inserido como filho IMEDIATO do elemento pai do que está sendo assinado.
/// Para NFe, o pai é `&lt;NFe&gt;` e o filho é `&lt;infNFe Id="NFe..."&gt;`. A `Signature` vai como
/// segundo filho de `&lt;NFe&gt;`, depois de `infNFe`.
/// </summary>
public sealed class XmlSignerC14N
{
    /// <summary>
    /// Assina um XML in-place adicionando &lt;Signature&gt; como filho do elemento que contém o `idElemento`.
    /// </summary>
    /// <param name="xml">XML completo (NFe, evento, inutNFe).</param>
    /// <param name="idElemento">Valor do atributo `Id` (sem `#`). Ex.: "NFe35260512345..."</param>
    /// <param name="cert">Certificado A1/A3 com chave privada acessível.</param>
    /// <returns>XML assinado como string UTF-8.</returns>
    public string Sign(string xml, string idElemento, X509Certificate2 cert)
    {
        if (!cert.HasPrivateKey)
            throw new InvalidOperationException("Certificado não tem chave privada acessível para assinar.");

        var doc = new XmlDocument { PreserveWhitespace = false };
        doc.LoadXml(xml);

        var elementoAssinar = LocalizarPorId(doc, idElemento)
            ?? throw new InvalidOperationException($"Elemento com Id='{idElemento}' não encontrado no XML.");

        var pai = elementoAssinar.ParentNode!
            ?? throw new InvalidOperationException("Elemento a assinar não tem pai — XML mal-formado.");

        // SignedXml por padrão olha xml:id ou Id; explicitamos via XmlResolver custom seria overkill.
        // Usamos sobrescrita de KeyedDocument: SignedXml(doc) localiza o Id por GetIdElement.
        var signedXml = new IdAwareSignedXml(doc, elementoAssinar)
        {
            SigningKey = cert.GetRSAPrivateKey()
                ?? throw new InvalidOperationException("Chave privada do cert não é RSA — apenas RSA é suportado pela SEFAZ."),
        };
        signedXml.SignedInfo!.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
        signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;

        var reference = new Reference
        {
            Uri = "#" + idElemento,
            DigestMethod = SignedXml.XmlDsigSHA1Url,
        };
        reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
        reference.AddTransform(new XmlDsigExcC14NTransform());
        signedXml.AddReference(reference);

        var keyInfo = new KeyInfo();
        keyInfo.AddClause(new KeyInfoX509Data(cert));
        signedXml.KeyInfo = keyInfo;

        signedXml.ComputeSignature();
        var signatureElement = signedXml.GetXml();

        // Append Signature como último filho do elemento pai (logo após infNFe / infEvento / infInut).
        pai.AppendChild(doc.ImportNode(signatureElement, deep: true));

        return doc.OuterXml;
    }

    private static XmlElement? LocalizarPorId(XmlDocument doc, string id)
    {
        // GetElementById só funciona se o XSD declarar o atributo como ID — nem sempre é o caso
        // em NFe sem schema atrelado. Buscamos manualmente.
        var todos = doc.GetElementsByTagName("*");
        foreach (XmlElement el in todos)
        {
            if (el.GetAttribute("Id") == id) return el;
        }
        return null;
    }

    /// <summary>
    /// Subclasse interna de SignedXml que ensina o framework a resolver `#id` para o elemento
    /// (NFe não usa xml:id, usa atributo "Id").
    /// </summary>
    private sealed class IdAwareSignedXml : SignedXml
    {
        private readonly XmlElement _alvo;

        public IdAwareSignedXml(XmlDocument doc, XmlElement alvo) : base(doc)
        {
            _alvo = alvo;
        }

        public override XmlElement? GetIdElement(XmlDocument? document, string idValue)
        {
            if (document is null) return null;
            // Match contra o alvo conhecido primeiro (otimização + segurança)
            var idAttr = _alvo.GetAttribute("Id");
            if (idAttr == idValue) return _alvo;

            return base.GetIdElement(document, idValue);
        }
    }
}
