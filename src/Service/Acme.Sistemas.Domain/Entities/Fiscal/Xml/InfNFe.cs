using System.Xml.Serialization;

namespace Acme.Sistemas.Domain.Entities.Fiscal.Xml;

/// <summary>
/// `infNFe` — composite raiz com 11 grupos do leiaute NF-e v4.00.
/// </summary>
public sealed class InfNFe
{
    /// <summary>Atributo `Id` no formato `NFe<chave-44-digitos>`. Usado como URI da assinatura.</summary>
    [XmlAttribute("Id")]
    public string Id { get; set; } = string.Empty;

    [XmlAttribute("versao")]
    public string Versao { get; set; } = NFeNamespaces.Versao;

    [XmlElement("ide")]
    public Ide Ide { get; set; } = new();

    [XmlElement("emit")]
    public Emit Emit { get; set; } = new();

    [XmlElement("dest")]
    public Dest? Dest { get; set; }

    [XmlElement("det")]
    public List<Det> Det { get; set; } = new();

    [XmlElement("total")]
    public Total Total { get; set; } = new();

    [XmlElement("transp")]
    public Transp Transp { get; set; } = new();

    [XmlElement("cobr")]
    public Cobr? Cobr { get; set; }

    [XmlElement("pag")]
    public Pag Pag { get; set; } = new();

    [XmlElement("infAdic")]
    public InfAdic? InfAdic { get; set; }

    /// <summary>Exporta — uso restrito a NFe de exportação (não modelado em profundidade).</summary>
    [XmlElement("exporta")]
    public Exporta? Exporta { get; set; }

    /// <summary>Compra — pedido/contrato em compras governamentais (raro).</summary>
    [XmlElement("compra")]
    public Compra? Compra { get; set; }

    /// <summary>Informações do responsável técnico (obrigatório desde NT 2018.005).</summary>
    [XmlElement("infRespTec")]
    public InfRespTec? InfRespTec { get; set; }
}

public sealed class Exporta
{
    [XmlElement("UFSaidaPais")] public string? UFSaidaPais { get; set; }
    [XmlElement("xLocExporta")] public string? XLocExporta { get; set; }
    [XmlElement("xLocDespacho")] public string? XLocDespacho { get; set; }
}

public sealed class Compra
{
    [XmlElement("xNEmp")] public string? XNEmp { get; set; }
    [XmlElement("xPed")] public string? XPed { get; set; }
    [XmlElement("xCont")] public string? XCont { get; set; }
}

public sealed class InfRespTec
{
    [XmlElement("CNPJ")] public string CNPJ { get; set; } = string.Empty;
    [XmlElement("xContato")] public string XContato { get; set; } = string.Empty;
    [XmlElement("email")] public string Email { get; set; } = string.Empty;
    [XmlElement("fone")] public string Fone { get; set; } = string.Empty;
    [XmlElement("idCSRT")] public string? IdCSRT { get; set; }
    [XmlElement("hashCSRT")] public string? HashCSRT { get; set; }
}
