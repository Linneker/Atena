using System.Xml.Serialization;

namespace Acme.Sistemas.Domain.Entities.Fiscal.Xml.Servicos;

/// <summary>
/// Payload de envio para `NFeAutorizacao4` — lote de NF-e (1..50 NFes).
/// </summary>
[XmlRoot("enviNFe", Namespace = NFeNamespaces.Portal)]
public sealed class EnviNFe
{
    [XmlAttribute("versao")] public string Versao { get; set; } = NFeNamespaces.Versao;

    [XmlElement("idLote")] public string IdLote { get; set; } = "1";

    /// <summary>0=Assíncrono (retorna recibo, polling depois), 1=Síncrono (retorna protocolo direto).</summary>
    [XmlElement("indSinc")] public string IndSinc { get; set; } = "1";

    [XmlElement("NFe")] public List<NFe> NFe { get; set; } = new();
}

/// <summary>
/// Resposta de `NFeAutorizacao4`. Pode conter:
/// - cStat=104 (lote processado) com `protNFe` por NFe — modo síncrono.
/// - cStat=103 (lote recebido) com `infRec.nRec` — modo assíncrono, polling depois.
/// </summary>
[XmlRoot("retEnviNFe", Namespace = NFeNamespaces.Portal)]
public sealed class RetEnviNFe
{
    [XmlAttribute("versao")] public string Versao { get; set; } = NFeNamespaces.Versao;

    [XmlElement("tpAmb")] public string TpAmb { get; set; } = string.Empty;
    [XmlElement("verAplic")] public string VerAplic { get; set; } = string.Empty;
    [XmlElement("cStat")] public string CStat { get; set; } = string.Empty;
    [XmlElement("xMotivo")] public string XMotivo { get; set; } = string.Empty;
    [XmlElement("cUF")] public string? CUF { get; set; }
    [XmlElement("dhRecbto")] public string? DhRecbto { get; set; }

    [XmlElement("infRec")] public InfRec? InfRec { get; set; }
    [XmlElement("protNFe")] public List<ProtNFe>? ProtNFe { get; set; }
}

public sealed class InfRec
{
    [XmlElement("nRec")] public string NRec { get; set; } = string.Empty;
    [XmlElement("tMed")] public string? TMed { get; set; }
}

/// <summary>
/// Payload de consulta de recibo — `NFeRetAutorizacao4`.
/// </summary>
[XmlRoot("consReciNFe", Namespace = NFeNamespaces.Portal)]
public sealed class ConsReciNFe
{
    [XmlAttribute("versao")] public string Versao { get; set; } = NFeNamespaces.Versao;
    [XmlElement("tpAmb")] public string TpAmb { get; set; } = "2";
    [XmlElement("nRec")] public string NRec { get; set; } = string.Empty;
}

[XmlRoot("retConsReciNFe", Namespace = NFeNamespaces.Portal)]
public sealed class RetConsReciNFe
{
    [XmlAttribute("versao")] public string Versao { get; set; } = NFeNamespaces.Versao;
    [XmlElement("tpAmb")] public string TpAmb { get; set; } = string.Empty;
    [XmlElement("verAplic")] public string VerAplic { get; set; } = string.Empty;
    [XmlElement("nRec")] public string NRec { get; set; } = string.Empty;
    [XmlElement("cStat")] public string CStat { get; set; } = string.Empty;
    [XmlElement("xMotivo")] public string XMotivo { get; set; } = string.Empty;
    [XmlElement("cUF")] public string? CUF { get; set; }
    [XmlElement("dhRecbto")] public string? DhRecbto { get; set; }
    [XmlElement("protNFe")] public List<ProtNFe>? ProtNFe { get; set; }
}
