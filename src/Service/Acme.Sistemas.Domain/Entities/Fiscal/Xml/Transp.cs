using System.Xml.Serialization;

namespace Acme.Sistemas.Domain.Entities.Fiscal.Xml;

/// <summary>
/// `transp` — transporte (X01). Apenas `modFrete` é obrigatório; demais blocos são opcionais.
/// </summary>
public sealed class Transp
{
    [XmlElement("modFrete")] public ModFrete ModFrete { get; set; } = ModFrete.SemFrete;

    [XmlElement("transporta")] public Transporta? Transporta { get; set; }

    [XmlElement("vol")] public List<Vol>? Vol { get; set; }
}

public sealed class Transporta
{
    [XmlElement("CNPJ")] public string? CNPJ { get; set; }
    [XmlElement("CPF")] public string? CPF { get; set; }
    [XmlElement("xNome")] public string? XNome { get; set; }
    [XmlElement("IE")] public string? IE { get; set; }
    [XmlElement("xEnder")] public string? XEnder { get; set; }
    [XmlElement("xMun")] public string? XMun { get; set; }
    [XmlElement("UF")] public string? UF { get; set; }
}

public sealed class Vol
{
    [XmlElement("qVol")] public string? QVol { get; set; }
    [XmlElement("esp")] public string? Esp { get; set; }
    [XmlElement("marca")] public string? Marca { get; set; }
    [XmlElement("nVol")] public string? NVol { get; set; }
    [XmlElement("pesoL")] public string? PesoL { get; set; }
    [XmlElement("pesoB")] public string? PesoB { get; set; }
}
