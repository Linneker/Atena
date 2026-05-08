using System.Xml.Serialization;

namespace Acme.Sistemas.Domain.Entities.Fiscal.Xml;

/// <summary>
/// Wrapper "procNFe" — NFe autorizada + protocolo de autorização.
/// É a forma final que circula após `cStat=100`.
/// </summary>
[XmlRoot("nfeProc", Namespace = NFeNamespaces.ProcNFe)]
public sealed class NFeProc
{
    [XmlAttribute("versao")]
    public string Versao { get; set; } = NFeNamespaces.Versao;

    [XmlElement("NFe")]
    public NFe NFe { get; set; } = new();

    [XmlElement("protNFe")]
    public ProtNFe ProtNFe { get; set; } = new();
}

public sealed class ProtNFe
{
    [XmlAttribute("versao")]
    public string Versao { get; set; } = NFeNamespaces.Versao;

    [XmlElement("infProt")]
    public InfProt InfProt { get; set; } = new();
}

public sealed class InfProt
{
    [XmlAttribute("Id")]
    public string? Id { get; set; }

    [XmlElement("tpAmb")]
    public TpAmb TpAmb { get; set; }

    [XmlElement("verAplic")]
    public string VerAplic { get; set; } = string.Empty;

    [XmlElement("chNFe")]
    public string ChNFe { get; set; } = string.Empty;

    [XmlElement("dhRecbto")]
    public DateTime DhRecbto { get; set; }

    [XmlElement("nProt")]
    public string NProt { get; set; } = string.Empty;

    [XmlElement("digVal")]
    public string DigVal { get; set; } = string.Empty;

    [XmlElement("cStat")]
    public string CStat { get; set; } = string.Empty;

    [XmlElement("xMotivo")]
    public string XMotivo { get; set; } = string.Empty;
}
