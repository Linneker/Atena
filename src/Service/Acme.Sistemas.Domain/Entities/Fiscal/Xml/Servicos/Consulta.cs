using System.Xml.Serialization;

namespace Acme.Sistemas.Domain.Entities.Fiscal.Xml.Servicos;

/// <summary>Consulta protocolo de NF-e por chave (`NFeConsultaProtocolo4`).</summary>
[XmlRoot("consSitNFe", Namespace = NFeNamespaces.Portal)]
public sealed class ConsSitNFe
{
    [XmlAttribute("versao")] public string Versao { get; set; } = NFeNamespaces.Versao;
    [XmlElement("tpAmb")] public string TpAmb { get; set; } = "2";
    [XmlElement("xServ")] public string XServ { get; set; } = "CONSULTAR";
    [XmlElement("chNFe")] public string ChNFe { get; set; } = string.Empty;
}

[XmlRoot("retConsSitNFe", Namespace = NFeNamespaces.Portal)]
public sealed class RetConsSitNFe
{
    [XmlAttribute("versao")] public string Versao { get; set; } = NFeNamespaces.Versao;
    [XmlElement("tpAmb")] public string TpAmb { get; set; } = string.Empty;
    [XmlElement("verAplic")] public string VerAplic { get; set; } = string.Empty;
    [XmlElement("cStat")] public string CStat { get; set; } = string.Empty;
    [XmlElement("xMotivo")] public string XMotivo { get; set; } = string.Empty;
    [XmlElement("chNFe")] public string? ChNFe { get; set; }
    [XmlElement("protNFe")] public ProtNFe? ProtNFe { get; set; }
}

/// <summary>Consulta status do serviço (`NFeStatusServico4`).</summary>
[XmlRoot("consStatServ", Namespace = NFeNamespaces.Portal)]
public sealed class ConsStatServ
{
    [XmlAttribute("versao")] public string Versao { get; set; } = NFeNamespaces.Versao;
    [XmlElement("tpAmb")] public string TpAmb { get; set; } = "2";
    [XmlElement("cUF")] public string CUF { get; set; } = string.Empty;
    [XmlElement("xServ")] public string XServ { get; set; } = "STATUS";
}

[XmlRoot("retConsStatServ", Namespace = NFeNamespaces.Portal)]
public sealed class RetConsStatServ
{
    [XmlAttribute("versao")] public string Versao { get; set; } = NFeNamespaces.Versao;
    [XmlElement("tpAmb")] public string TpAmb { get; set; } = string.Empty;
    [XmlElement("verAplic")] public string VerAplic { get; set; } = string.Empty;
    [XmlElement("cStat")] public string CStat { get; set; } = string.Empty;
    [XmlElement("xMotivo")] public string XMotivo { get; set; } = string.Empty;
    [XmlElement("cUF")] public string? CUF { get; set; }
    [XmlElement("dhRecbto")] public string? DhRecbto { get; set; }
    [XmlElement("tMed")] public string? TMed { get; set; }
}
