using System.Xml.Serialization;

namespace Acme.Sistemas.Domain.Entities.Fiscal.Xml;

/// <summary>
/// `total` — totais da NF-e (W01). Composto por ICMSTot e opcionalmente ISSQNtot e retTrib.
/// </summary>
public sealed class Total
{
    [XmlElement("ICMSTot")] public ICMSTot ICMSTot { get; set; } = new();

    [XmlElement("ISSQNtot")] public ISSQNtot? ISSQNtot { get; set; }

    [XmlElement("retTrib")] public RetTrib? RetTrib { get; set; }
}

public sealed class ICMSTot
{
    [XmlElement("vBC")] public string VBC { get; set; } = "0.00";
    [XmlElement("vICMS")] public string VICMS { get; set; } = "0.00";
    [XmlElement("vICMSDeson")] public string VICMSDeson { get; set; } = "0.00";
    [XmlElement("vFCP")] public string VFCP { get; set; } = "0.00";
    [XmlElement("vBCST")] public string VBCST { get; set; } = "0.00";
    [XmlElement("vST")] public string VST { get; set; } = "0.00";
    [XmlElement("vFCPST")] public string VFCPST { get; set; } = "0.00";
    [XmlElement("vFCPSTRet")] public string VFCPSTRet { get; set; } = "0.00";
    [XmlElement("vProd")] public string VProd { get; set; } = "0.00";
    [XmlElement("vFrete")] public string VFrete { get; set; } = "0.00";
    [XmlElement("vSeg")] public string VSeg { get; set; } = "0.00";
    [XmlElement("vDesc")] public string VDesc { get; set; } = "0.00";
    [XmlElement("vII")] public string VII { get; set; } = "0.00";
    [XmlElement("vIPI")] public string VIPI { get; set; } = "0.00";
    [XmlElement("vIPIDevol")] public string VIPIDevol { get; set; } = "0.00";
    [XmlElement("vPIS")] public string VPIS { get; set; } = "0.00";
    [XmlElement("vCOFINS")] public string VCOFINS { get; set; } = "0.00";
    [XmlElement("vOutro")] public string VOutro { get; set; } = "0.00";
    [XmlElement("vNF")] public string VNF { get; set; } = "0.00";
    [XmlElement("vTotTrib")] public string? VTotTrib { get; set; }
}

public sealed class ISSQNtot
{
    [XmlElement("vServ")] public string VServ { get; set; } = "0.00";
    [XmlElement("vBC")] public string VBC { get; set; } = "0.00";
    [XmlElement("vISS")] public string VISS { get; set; } = "0.00";
    [XmlElement("vPIS")] public string VPIS { get; set; } = "0.00";
    [XmlElement("vCOFINS")] public string VCOFINS { get; set; } = "0.00";
}

public sealed class RetTrib
{
    [XmlElement("vRetPIS")] public string? VRetPIS { get; set; }
    [XmlElement("vRetCOFINS")] public string? VRetCOFINS { get; set; }
    [XmlElement("vRetCSLL")] public string? VRetCSLL { get; set; }
    [XmlElement("vBCIRRF")] public string? VBCIRRF { get; set; }
    [XmlElement("vIRRF")] public string? VIRRF { get; set; }
    [XmlElement("vBCRetPrev")] public string? VBCRetPrev { get; set; }
    [XmlElement("vRetPrev")] public string? VRetPrev { get; set; }
}
