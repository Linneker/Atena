using System.Xml.Serialization;

namespace Acme.Sistemas.Domain.Entities.Fiscal.Xml;

/// <summary>
/// `det` — item da NF-e (H01). NFe pode conter de 1 a 990 itens.
/// </summary>
public sealed class Det
{
    [XmlAttribute("nItem")] public string NItem { get; set; } = string.Empty;

    [XmlElement("prod")] public Prod Prod { get; set; } = new();

    [XmlElement("imposto")] public Imposto Imposto { get; set; } = new();

    [XmlElement("infAdProd")] public string? InfAdProd { get; set; }
}

/// <summary>
/// `prod` — produto/serviço (I01).
/// </summary>
public sealed class Prod
{
    [XmlElement("cProd")] public string CProd { get; set; } = string.Empty;
    [XmlElement("cEAN")] public string CEAN { get; set; } = "SEM GTIN";
    [XmlElement("xProd")] public string XProd { get; set; } = string.Empty;
    /// <summary>Nomenclatura Comum do Mercosul (8 dígitos).</summary>
    [XmlElement("NCM")] public string NCM { get; set; } = string.Empty;
    /// <summary>Código fiscal de operações e prestações (4 dígitos).</summary>
    [XmlElement("CFOP")] public string CFOP { get; set; } = string.Empty;
    [XmlElement("uCom")] public string UCom { get; set; } = string.Empty;
    [XmlElement("qCom")] public string QCom { get; set; } = string.Empty;
    [XmlElement("vUnCom")] public string VUnCom { get; set; } = string.Empty;
    [XmlElement("vProd")] public string VProd { get; set; } = string.Empty;
    [XmlElement("cEANTrib")] public string CEANTrib { get; set; } = "SEM GTIN";
    [XmlElement("uTrib")] public string UTrib { get; set; } = string.Empty;
    [XmlElement("qTrib")] public string QTrib { get; set; } = string.Empty;
    [XmlElement("vUnTrib")] public string VUnTrib { get; set; } = string.Empty;
    [XmlElement("vFrete")] public string? VFrete { get; set; }
    [XmlElement("vSeg")] public string? VSeg { get; set; }
    [XmlElement("vDesc")] public string? VDesc { get; set; }
    [XmlElement("vOutro")] public string? VOutro { get; set; }
    /// <summary>Indica se o item compõe o valor total da NF-e (0=Não, 1=Sim).</summary>
    [XmlElement("indTot")] public string IndTot { get; set; } = "1";
    [XmlElement("CEST")] public string? CEST { get; set; }
}
