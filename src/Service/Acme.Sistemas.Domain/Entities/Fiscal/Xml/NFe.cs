using System.Xml.Serialization;

namespace Acme.Sistemas.Domain.Entities.Fiscal.Xml;

/// <summary>
/// Envelope NFe — contém infNFe (dados) e Signature (XMLDSig).
/// O nome do tipo conflita com a entidade de domínio existente; este vive em
/// namespace .Xml e é usado apenas para serialização.
/// </summary>
[XmlRoot("NFe", Namespace = NFeNamespaces.Portal)]
public sealed class NFe
{
    [XmlElement("infNFe")]
    public InfNFe InfNFe { get; set; } = new();

    /// <summary>
    /// Assinatura XMLDSig embutida após assinar. Ignored se nula na serialização inicial.
    /// </summary>
    [XmlElement("Signature", Namespace = NFeNamespaces.XmlDsig)]
    public Signature? Signature { get; set; }

    [XmlIgnore]
    public bool SignatureSpecified => Signature is not null;
}

/// <summary>
/// Placeholder estrutural do bloco XMLDSig que o `XmlSignerC14N` (Fase 2) preenche.
/// Não modelado em detalhe aqui — assinatura é manipulada via `SignedXml` do .NET, que
/// produz o XML pronto.
/// </summary>
public sealed class Signature
{
    [XmlAnyElement]
    public System.Xml.XmlElement[]? Any { get; set; }
}
