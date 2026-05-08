using System.Xml.Serialization;

namespace Acme.Sistemas.Domain.Entities.Fiscal.Xml;

/// <summary>
/// `imposto` — composite de tributos do item (M01).
/// Cada tributo (ICMS, IPI, PIS, COFINS) tem múltiplas variantes mutuamente exclusivas.
/// O modelo usa `XmlChoice` para refletir a escolha exclusiva onde aplicável e wrappers
/// para grupos repetitivos.
/// </summary>
public sealed class Imposto
{
    /// <summary>Valor aproximado total dos tributos (Lei 12741/2012).</summary>
    [XmlElement("vTotTrib")] public string? VTotTrib { get; set; }

    [XmlElement("ICMS")] public ICMS ICMS { get; set; } = new();

    [XmlElement("IPI")] public IPI? IPI { get; set; }

    [XmlElement("PIS")] public PIS PIS { get; set; } = new();

    [XmlElement("COFINS")] public COFINS COFINS { get; set; } = new();

    [XmlElement("ISSQN")] public ISSQN? ISSQN { get; set; }
}

/// <summary>
/// Wrapper ICMS — exatamente UM dos sub-grupos é preenchido por item (ICMS00, 10, 20, ..., 90, SN101, SN102, etc.).
/// Modelagem completa de todos os ~25 grupos é volumosa; o cliente deve preencher um por item.
/// </summary>
public sealed class ICMS
{
    [XmlElement("ICMS00")] public ICMS00? ICMS00 { get; set; }
    [XmlElement("ICMS10")] public ICMS10? ICMS10 { get; set; }
    [XmlElement("ICMS20")] public ICMS20? ICMS20 { get; set; }
    [XmlElement("ICMS30")] public ICMS30? ICMS30 { get; set; }
    [XmlElement("ICMS40")] public ICMS40? ICMS40 { get; set; }
    [XmlElement("ICMS51")] public ICMS51? ICMS51 { get; set; }
    [XmlElement("ICMS60")] public ICMS60? ICMS60 { get; set; }
    [XmlElement("ICMS70")] public ICMS70? ICMS70 { get; set; }
    [XmlElement("ICMS90")] public ICMS90? ICMS90 { get; set; }
    [XmlElement("ICMSSN101")] public ICMSSN101? ICMSSN101 { get; set; }
    [XmlElement("ICMSSN102")] public ICMSSN102? ICMSSN102 { get; set; }
    [XmlElement("ICMSSN201")] public ICMSSN201? ICMSSN201 { get; set; }
    // TODO Fase 4 — ICMSSN202/500/900, ICMS41/50/61/ST, ICMSPart, ICMSRep, etc.
    // Cada variante adicional é mecanicamente análoga aos modelados aqui.
}

/// <summary>Tributação integral (CST 00).</summary>
public sealed class ICMS00
{
    [XmlElement("orig")] public string Orig { get; set; } = "0";
    [XmlElement("CST")] public string CST { get; set; } = "00";
    [XmlElement("modBC")] public string ModBC { get; set; } = "3";
    [XmlElement("vBC")] public string VBC { get; set; } = "0.00";
    [XmlElement("pICMS")] public string PICMS { get; set; } = "0.00";
    [XmlElement("vICMS")] public string VICMS { get; set; } = "0.00";
    [XmlElement("pFCP")] public string? PFCP { get; set; }
    [XmlElement("vFCP")] public string? VFCP { get; set; }
}

/// <summary>Tributada e com cobrança do ICMS por substituição tributária (CST 10).</summary>
public sealed class ICMS10
{
    [XmlElement("orig")] public string Orig { get; set; } = "0";
    [XmlElement("CST")] public string CST { get; set; } = "10";
    [XmlElement("modBC")] public string ModBC { get; set; } = "3";
    [XmlElement("vBC")] public string VBC { get; set; } = "0.00";
    [XmlElement("pICMS")] public string PICMS { get; set; } = "0.00";
    [XmlElement("vICMS")] public string VICMS { get; set; } = "0.00";
    [XmlElement("modBCST")] public string ModBCST { get; set; } = "4";
    [XmlElement("pMVAST")] public string? PMVAST { get; set; }
    [XmlElement("pRedBCST")] public string? PRedBCST { get; set; }
    [XmlElement("vBCST")] public string VBCST { get; set; } = "0.00";
    [XmlElement("pICMSST")] public string PICMSST { get; set; } = "0.00";
    [XmlElement("vICMSST")] public string VICMSST { get; set; } = "0.00";
}

/// <summary>Com redução de base de cálculo (CST 20).</summary>
public sealed class ICMS20
{
    [XmlElement("orig")] public string Orig { get; set; } = "0";
    [XmlElement("CST")] public string CST { get; set; } = "20";
    [XmlElement("modBC")] public string ModBC { get; set; } = "3";
    [XmlElement("pRedBC")] public string PRedBC { get; set; } = "0.00";
    [XmlElement("vBC")] public string VBC { get; set; } = "0.00";
    [XmlElement("pICMS")] public string PICMS { get; set; } = "0.00";
    [XmlElement("vICMS")] public string VICMS { get; set; } = "0.00";
    [XmlElement("vICMSDeson")] public string? VICMSDeson { get; set; }
    [XmlElement("motDesICMS")] public string? MotDesICMS { get; set; }
}

/// <summary>Isenta ou não tributada e com cobrança do ICMS por ST (CST 30).</summary>
public sealed class ICMS30
{
    [XmlElement("orig")] public string Orig { get; set; } = "0";
    [XmlElement("CST")] public string CST { get; set; } = "30";
    [XmlElement("modBCST")] public string ModBCST { get; set; } = "4";
    [XmlElement("vBCST")] public string VBCST { get; set; } = "0.00";
    [XmlElement("pICMSST")] public string PICMSST { get; set; } = "0.00";
    [XmlElement("vICMSST")] public string VICMSST { get; set; } = "0.00";
    [XmlElement("vICMSDeson")] public string? VICMSDeson { get; set; }
    [XmlElement("motDesICMS")] public string? MotDesICMS { get; set; }
}

/// <summary>Isenta, não tributada ou suspensão (CST 40, 41, 50).</summary>
public sealed class ICMS40
{
    [XmlElement("orig")] public string Orig { get; set; } = "0";
    [XmlElement("CST")] public string CST { get; set; } = "40";
    [XmlElement("vICMSDeson")] public string? VICMSDeson { get; set; }
    [XmlElement("motDesICMS")] public string? MotDesICMS { get; set; }
}

/// <summary>Diferimento (CST 51).</summary>
public sealed class ICMS51
{
    [XmlElement("orig")] public string Orig { get; set; } = "0";
    [XmlElement("CST")] public string CST { get; set; } = "51";
    [XmlElement("modBC")] public string? ModBC { get; set; }
    [XmlElement("pRedBC")] public string? PRedBC { get; set; }
    [XmlElement("vBC")] public string? VBC { get; set; }
    [XmlElement("pICMS")] public string? PICMS { get; set; }
    [XmlElement("vICMSOp")] public string? VICMSOp { get; set; }
    [XmlElement("pDif")] public string? PDif { get; set; }
    [XmlElement("vICMSDif")] public string? VICMSDif { get; set; }
    [XmlElement("vICMS")] public string? VICMS { get; set; }
}

/// <summary>ICMS cobrado anteriormente por ST (CST 60).</summary>
public sealed class ICMS60
{
    [XmlElement("orig")] public string Orig { get; set; } = "0";
    [XmlElement("CST")] public string CST { get; set; } = "60";
    [XmlElement("vBCSTRet")] public string? VBCSTRet { get; set; }
    [XmlElement("pST")] public string? PST { get; set; }
    [XmlElement("vICMSSubstituto")] public string? VICMSSubstituto { get; set; }
    [XmlElement("vICMSSTRet")] public string? VICMSSTRet { get; set; }
}

/// <summary>Com redução de BC e cobrança do ICMS por ST (CST 70).</summary>
public sealed class ICMS70
{
    [XmlElement("orig")] public string Orig { get; set; } = "0";
    [XmlElement("CST")] public string CST { get; set; } = "70";
    [XmlElement("modBC")] public string ModBC { get; set; } = "3";
    [XmlElement("pRedBC")] public string PRedBC { get; set; } = "0.00";
    [XmlElement("vBC")] public string VBC { get; set; } = "0.00";
    [XmlElement("pICMS")] public string PICMS { get; set; } = "0.00";
    [XmlElement("vICMS")] public string VICMS { get; set; } = "0.00";
    [XmlElement("modBCST")] public string ModBCST { get; set; } = "4";
    [XmlElement("vBCST")] public string VBCST { get; set; } = "0.00";
    [XmlElement("pICMSST")] public string PICMSST { get; set; } = "0.00";
    [XmlElement("vICMSST")] public string VICMSST { get; set; } = "0.00";
}

/// <summary>Outras (CST 90).</summary>
public sealed class ICMS90
{
    [XmlElement("orig")] public string Orig { get; set; } = "0";
    [XmlElement("CST")] public string CST { get; set; } = "90";
    [XmlElement("modBC")] public string? ModBC { get; set; }
    [XmlElement("vBC")] public string? VBC { get; set; }
    [XmlElement("pRedBC")] public string? PRedBC { get; set; }
    [XmlElement("pICMS")] public string? PICMS { get; set; }
    [XmlElement("vICMS")] public string? VICMS { get; set; }
}

/// <summary>Simples Nacional — Tributada com permissão de crédito (CSOSN 101).</summary>
public sealed class ICMSSN101
{
    [XmlElement("orig")] public string Orig { get; set; } = "0";
    [XmlElement("CSOSN")] public string CSOSN { get; set; } = "101";
    [XmlElement("pCredSN")] public string PCredSN { get; set; } = "0.00";
    [XmlElement("vCredICMSSN")] public string VCredICMSSN { get; set; } = "0.00";
}

/// <summary>Simples Nacional — Tributada sem permissão de crédito (CSOSN 102/103/300/400).</summary>
public sealed class ICMSSN102
{
    [XmlElement("orig")] public string Orig { get; set; } = "0";
    [XmlElement("CSOSN")] public string CSOSN { get; set; } = "102";
}

/// <summary>Simples Nacional — Tributada com permissão de crédito + ST (CSOSN 201).</summary>
public sealed class ICMSSN201
{
    [XmlElement("orig")] public string Orig { get; set; } = "0";
    [XmlElement("CSOSN")] public string CSOSN { get; set; } = "201";
    [XmlElement("modBCST")] public string ModBCST { get; set; } = "4";
    [XmlElement("vBCST")] public string VBCST { get; set; } = "0.00";
    [XmlElement("pICMSST")] public string PICMSST { get; set; } = "0.00";
    [XmlElement("vICMSST")] public string VICMSST { get; set; } = "0.00";
    [XmlElement("pCredSN")] public string PCredSN { get; set; } = "0.00";
    [XmlElement("vCredICMSSN")] public string VCredICMSSN { get; set; } = "0.00";
}

public sealed class IPI
{
    [XmlElement("cEnq")] public string CEnq { get; set; } = "999";
    [XmlElement("IPITrib")] public IPITrib? IPITrib { get; set; }
    [XmlElement("IPINT")] public IPINT? IPINT { get; set; }
}

public sealed class IPITrib
{
    [XmlElement("CST")] public string CST { get; set; } = "00";
    [XmlElement("vBC")] public string? VBC { get; set; }
    [XmlElement("pIPI")] public string? PIPI { get; set; }
    [XmlElement("vIPI")] public string VIPI { get; set; } = "0.00";
}

public sealed class IPINT
{
    [XmlElement("CST")] public string CST { get; set; } = "01";
}

public sealed class PIS
{
    [XmlElement("PISAliq")] public PISAliq? PISAliq { get; set; }
    [XmlElement("PISNT")] public PISNT? PISNT { get; set; }
    [XmlElement("PISOutr")] public PISOutr? PISOutr { get; set; }
}

public sealed class PISAliq
{
    [XmlElement("CST")] public string CST { get; set; } = "01";
    [XmlElement("vBC")] public string VBC { get; set; } = "0.00";
    [XmlElement("pPIS")] public string PPIS { get; set; } = "0.00";
    [XmlElement("vPIS")] public string VPIS { get; set; } = "0.00";
}

public sealed class PISNT
{
    [XmlElement("CST")] public string CST { get; set; } = "07";
}

public sealed class PISOutr
{
    [XmlElement("CST")] public string CST { get; set; } = "99";
    [XmlElement("vBC")] public string? VBC { get; set; }
    [XmlElement("pPIS")] public string? PPIS { get; set; }
    [XmlElement("vPIS")] public string VPIS { get; set; } = "0.00";
}

public sealed class COFINS
{
    [XmlElement("COFINSAliq")] public COFINSAliq? COFINSAliq { get; set; }
    [XmlElement("COFINSNT")] public COFINSNT? COFINSNT { get; set; }
    [XmlElement("COFINSOutr")] public COFINSOutr? COFINSOutr { get; set; }
}

public sealed class COFINSAliq
{
    [XmlElement("CST")] public string CST { get; set; } = "01";
    [XmlElement("vBC")] public string VBC { get; set; } = "0.00";
    [XmlElement("pCOFINS")] public string PCOFINS { get; set; } = "0.00";
    [XmlElement("vCOFINS")] public string VCOFINS { get; set; } = "0.00";
}

public sealed class COFINSNT
{
    [XmlElement("CST")] public string CST { get; set; } = "07";
}

public sealed class COFINSOutr
{
    [XmlElement("CST")] public string CST { get; set; } = "99";
    [XmlElement("vBC")] public string? VBC { get; set; }
    [XmlElement("pCOFINS")] public string? PCOFINS { get; set; }
    [XmlElement("vCOFINS")] public string VCOFINS { get; set; } = "0.00";
}

public sealed class ISSQN
{
    [XmlElement("vBC")] public string VBC { get; set; } = "0.00";
    [XmlElement("vAliq")] public string VAliq { get; set; } = "0.00";
    [XmlElement("vISSQN")] public string VISSQN { get; set; } = "0.00";
    [XmlElement("cMunFG")] public string CMunFG { get; set; } = string.Empty;
    [XmlElement("cListServ")] public string CListServ { get; set; } = string.Empty;
}
