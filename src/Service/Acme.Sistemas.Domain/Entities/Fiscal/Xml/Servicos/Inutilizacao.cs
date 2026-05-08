using System.Xml.Serialization;

namespace Acme.Sistemas.Domain.Entities.Fiscal.Xml.Servicos;

/// <summary>
/// Inutilização de faixa de numeração — `NFeInutilizacao4`. Útil para descartar
/// numeração não-usada antes de encerramento mensal.
/// </summary>
[XmlRoot("inutNFe", Namespace = NFeNamespaces.Portal)]
public sealed class InutNFe
{
    [XmlAttribute("versao")] public string Versao { get; set; } = NFeNamespaces.Versao;
    [XmlElement("infInut")] public InfInut InfInut { get; set; } = new();

    [XmlElement("Signature", Namespace = NFeNamespaces.XmlDsig)]
    public Signature? Signature { get; set; }
}

public sealed class InfInut
{
    /// <summary>Atributo Id no formato `ID<cUF><ano><CNPJ><mod><serie><nNFIni><nNFFin>` (43 chars).</summary>
    [XmlAttribute("Id")] public string Id { get; set; } = string.Empty;

    [XmlElement("tpAmb")] public string TpAmb { get; set; } = "2";
    [XmlElement("xServ")] public string XServ { get; set; } = "INUTILIZAR";
    [XmlElement("cUF")] public string CUF { get; set; } = string.Empty;
    [XmlElement("ano")] public string Ano { get; set; } = string.Empty;
    [XmlElement("CNPJ")] public string CNPJ { get; set; } = string.Empty;
    [XmlElement("mod")] public string Mod { get; set; } = "55";
    [XmlElement("serie")] public string Serie { get; set; } = string.Empty;
    [XmlElement("nNFIni")] public string NNFIni { get; set; } = string.Empty;
    [XmlElement("nNFFin")] public string NNFFin { get; set; } = string.Empty;
    [XmlElement("xJust")] public string XJust { get; set; } = string.Empty;
}

[XmlRoot("retInutNFe", Namespace = NFeNamespaces.Portal)]
public sealed class RetInutNFe
{
    [XmlAttribute("versao")] public string Versao { get; set; } = NFeNamespaces.Versao;
    [XmlElement("infInut")] public InfInutResposta InfInut { get; set; } = new();
}

public sealed class InfInutResposta
{
    [XmlAttribute("Id")] public string? Id { get; set; }
    [XmlElement("tpAmb")] public string TpAmb { get; set; } = string.Empty;
    [XmlElement("verAplic")] public string VerAplic { get; set; } = string.Empty;
    [XmlElement("cStat")] public string CStat { get; set; } = string.Empty;
    [XmlElement("xMotivo")] public string XMotivo { get; set; } = string.Empty;
    [XmlElement("cUF")] public string? CUF { get; set; }
    [XmlElement("ano")] public string? Ano { get; set; }
    [XmlElement("CNPJ")] public string? CNPJ { get; set; }
    [XmlElement("mod")] public string? Mod { get; set; }
    [XmlElement("serie")] public string? Serie { get; set; }
    [XmlElement("nNFIni")] public string? NNFIni { get; set; }
    [XmlElement("nNFFin")] public string? NNFFin { get; set; }
    [XmlElement("dhRecbto")] public string? DhRecbto { get; set; }
    [XmlElement("nProt")] public string? NProt { get; set; }
}
