using System.Xml.Serialization;

namespace Acme.Sistemas.Domain.Entities.Fiscal.Xml;

/// <summary>
/// `cobr` — cobrança/duplicatas (Y01).
/// </summary>
public sealed class Cobr
{
    [XmlElement("fat")] public Fat? Fat { get; set; }
    [XmlElement("dup")] public List<Dup>? Dup { get; set; }
}

public sealed class Fat
{
    [XmlElement("nFat")] public string? NFat { get; set; }
    [XmlElement("vOrig")] public string? VOrig { get; set; }
    [XmlElement("vDesc")] public string? VDesc { get; set; }
    [XmlElement("vLiq")] public string? VLiq { get; set; }
}

public sealed class Dup
{
    [XmlElement("nDup")] public string NDup { get; set; } = string.Empty;
    [XmlElement("dVenc")] public string DVenc { get; set; } = string.Empty;
    [XmlElement("vDup")] public string VDup { get; set; } = "0.00";
}

/// <summary>
/// `pag` — meios de pagamento (YA01). Pelo menos um `detPag` é obrigatório.
/// </summary>
public sealed class Pag
{
    [XmlElement("detPag")] public List<DetPag> DetPag { get; set; } = new();
    [XmlElement("vTroco")] public string? VTroco { get; set; }
}

public sealed class DetPag
{
    [XmlElement("indPag")] public string? IndPag { get; set; }
    [XmlElement("tPag")] public TpPag TPag { get; set; } = TpPag.Dinheiro;
    [XmlElement("vPag")] public string VPag { get; set; } = "0.00";
    [XmlElement("card")] public Card? Card { get; set; }
}

public sealed class Card
{
    [XmlElement("tpIntegra")] public string TpIntegra { get; set; } = "2";
    [XmlElement("CNPJ")] public string? CNPJ { get; set; }
    [XmlElement("tBand")] public string? TBand { get; set; }
    [XmlElement("cAut")] public string? CAut { get; set; }
}

/// <summary>
/// `infAdic` — informações adicionais.
/// </summary>
public sealed class InfAdic
{
    /// <summary>Informações adicionais de interesse do Fisco (texto livre).</summary>
    [XmlElement("infAdFisco")] public string? InfAdFisco { get; set; }

    /// <summary>Informações complementares de interesse do contribuinte.</summary>
    [XmlElement("infCpl")] public string? InfCpl { get; set; }
}
