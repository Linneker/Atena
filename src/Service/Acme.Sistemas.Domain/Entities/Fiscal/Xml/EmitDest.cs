using System.Xml.Serialization;

namespace Acme.Sistemas.Domain.Entities.Fiscal.Xml;

/// <summary>
/// `emit` — emitente (C01 do layout). CNPJ obrigatório (CPF apenas em casos especiais).
/// </summary>
public sealed class Emit
{
    [XmlElement("CNPJ")] public string? CNPJ { get; set; }
    [XmlElement("CPF")] public string? CPF { get; set; }
    [XmlElement("xNome")] public string XNome { get; set; } = string.Empty;
    [XmlElement("xFant")] public string? XFant { get; set; }
    [XmlElement("enderEmit")] public Endereco EnderEmit { get; set; } = new();
    [XmlElement("IE")] public string IE { get; set; } = string.Empty;
    [XmlElement("IEST")] public string? IEST { get; set; }
    [XmlElement("IM")] public string? IM { get; set; }
    [XmlElement("CNAE")] public string? CNAE { get; set; }
    [XmlElement("CRT")] public CRT CRT { get; set; } = CRT.RegimeNormal;
}

/// <summary>
/// `dest` — destinatário (E01). CPF/CNPJ/idEstrangeiro mutuamente exclusivos.
/// </summary>
public sealed class Dest
{
    [XmlElement("CNPJ")] public string? CNPJ { get; set; }
    [XmlElement("CPF")] public string? CPF { get; set; }
    [XmlElement("idEstrangeiro")] public string? IdEstrangeiro { get; set; }
    [XmlElement("xNome")] public string XNome { get; set; } = string.Empty;
    [XmlElement("enderDest")] public Endereco? EnderDest { get; set; }
    [XmlElement("indIEDest")] public IndIEDest IndIEDest { get; set; } = IndIEDest.NaoContribuinte;
    [XmlElement("IE")] public string? IE { get; set; }
    [XmlElement("ISUF")] public string? ISUF { get; set; }
    [XmlElement("IM")] public string? IM { get; set; }
    [XmlElement("email")] public string? Email { get; set; }
}

/// <summary>
/// Endereço comum a `enderEmit` e `enderDest`. Campos com mesmos nomes do XSD.
/// </summary>
public sealed class Endereco
{
    [XmlElement("xLgr")] public string XLgr { get; set; } = string.Empty;
    [XmlElement("nro")] public string Nro { get; set; } = string.Empty;
    [XmlElement("xCpl")] public string? XCpl { get; set; }
    [XmlElement("xBairro")] public string XBairro { get; set; } = string.Empty;
    /// <summary>Código IBGE 7 dígitos.</summary>
    [XmlElement("cMun")] public string CMun { get; set; } = string.Empty;
    [XmlElement("xMun")] public string XMun { get; set; } = string.Empty;
    /// <summary>UF (SP, RJ, ...). Para enderDest exterior, usar "EX".</summary>
    [XmlElement("UF")] public string UF { get; set; } = string.Empty;
    [XmlElement("CEP")] public string? CEP { get; set; }
    /// <summary>Código país BACEN (1058 = Brasil).</summary>
    [XmlElement("cPais")] public string? CPais { get; set; } = "1058";
    [XmlElement("xPais")] public string? XPais { get; set; } = "BRASIL";
    [XmlElement("fone")] public string? Fone { get; set; }
}
