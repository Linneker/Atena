namespace Acme.Sistemas.Domain.Entities.Fiscal.Xml;

/// <summary>
/// Namespaces XML oficiais usados pelos POCOs NF-e v4.00.
/// Referência: leiauteNFe_v4.00.xsd
/// </summary>
public static class NFeNamespaces
{
    public const string Portal = "http://www.portalfiscal.inf.br/nfe";
    public const string XmlDsig = "http://www.w3.org/2000/09/xmldsig#";
    public const string ProcNFe = Portal;
    public const string Versao = "4.00";
}
