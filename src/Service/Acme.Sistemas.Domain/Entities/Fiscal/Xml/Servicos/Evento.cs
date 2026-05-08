using System.Xml.Serialization;

namespace Acme.Sistemas.Domain.Entities.Fiscal.Xml.Servicos;

/// <summary>
/// Wrapper de envio de evento — `NFeRecepcaoEvento4`. Contém 1 ou mais eventos assinados.
/// </summary>
[XmlRoot("envEvento", Namespace = NFeNamespaces.Portal)]
public sealed class EnvEvento
{
    [XmlAttribute("versao")] public string Versao { get; set; } = "1.00";
    [XmlElement("idLote")] public string IdLote { get; set; } = "1";
    [XmlElement("evento")] public List<Evento> Evento { get; set; } = new();
}

public sealed class Evento
{
    [XmlAttribute("versao")] public string Versao { get; set; } = "1.00";
    [XmlElement("infEvento")] public InfEvento InfEvento { get; set; } = new();

    [XmlElement("Signature", Namespace = NFeNamespaces.XmlDsig)]
    public Signature? Signature { get; set; }
}

public sealed class InfEvento
{
    /// <summary>Atributo Id no formato `ID<tpEvento><chNFe><nSeqEvento>` (54 chars).</summary>
    [XmlAttribute("Id")] public string Id { get; set; } = string.Empty;

    [XmlElement("cOrgao")] public string COrgao { get; set; } = string.Empty;
    [XmlElement("tpAmb")] public string TpAmb { get; set; } = "2";
    [XmlElement("CNPJ")] public string? CNPJ { get; set; }
    [XmlElement("CPF")] public string? CPF { get; set; }
    [XmlElement("chNFe")] public string ChNFe { get; set; } = string.Empty;
    [XmlElement("dhEvento")] public string DhEvento { get; set; } = string.Empty;
    /// <summary>110110 = CC-e, 110111 = Cancelamento.</summary>
    [XmlElement("tpEvento")] public string TpEvento { get; set; } = string.Empty;
    [XmlElement("nSeqEvento")] public string NSeqEvento { get; set; } = "1";
    [XmlElement("verEvento")] public string VerEvento { get; set; } = "1.00";
    [XmlElement("detEvento")] public DetEvento DetEvento { get; set; } = new();
}

public sealed class DetEvento
{
    [XmlAttribute("versao")] public string Versao { get; set; } = "1.00";
    [XmlElement("descEvento")] public string DescEvento { get; set; } = string.Empty;

    // Cancelamento (tpEvento=110111)
    [XmlElement("nProt")] public string? NProt { get; set; }
    [XmlElement("xJust")] public string? XJust { get; set; }

    // CC-e (tpEvento=110110)
    [XmlElement("xCorrecao")] public string? XCorrecao { get; set; }
    [XmlElement("xCondUso")] public string? XCondUso { get; set; }
}

[XmlRoot("retEnvEvento", Namespace = NFeNamespaces.Portal)]
public sealed class RetEnvEvento
{
    [XmlAttribute("versao")] public string Versao { get; set; } = "1.00";
    [XmlElement("idLote")] public string IdLote { get; set; } = string.Empty;
    [XmlElement("tpAmb")] public string TpAmb { get; set; } = string.Empty;
    [XmlElement("verAplic")] public string VerAplic { get; set; } = string.Empty;
    [XmlElement("cOrgao")] public string COrgao { get; set; } = string.Empty;
    [XmlElement("cStat")] public string CStat { get; set; } = string.Empty;
    [XmlElement("xMotivo")] public string XMotivo { get; set; } = string.Empty;
    [XmlElement("retEvento")] public List<RetEvento>? RetEvento { get; set; }
}

public sealed class RetEvento
{
    [XmlAttribute("versao")] public string Versao { get; set; } = "1.00";
    [XmlElement("infEvento")] public InfEventoResposta InfEvento { get; set; } = new();
}

public sealed class InfEventoResposta
{
    [XmlAttribute("Id")] public string? Id { get; set; }
    [XmlElement("tpAmb")] public string TpAmb { get; set; } = string.Empty;
    [XmlElement("verAplic")] public string VerAplic { get; set; } = string.Empty;
    [XmlElement("cOrgao")] public string COrgao { get; set; } = string.Empty;
    [XmlElement("cStat")] public string CStat { get; set; } = string.Empty;
    [XmlElement("xMotivo")] public string XMotivo { get; set; } = string.Empty;
    [XmlElement("chNFe")] public string? ChNFe { get; set; }
    [XmlElement("tpEvento")] public string? TpEvento { get; set; }
    [XmlElement("xEvento")] public string? XEvento { get; set; }
    [XmlElement("nSeqEvento")] public string? NSeqEvento { get; set; }
    [XmlElement("CNPJDest")] public string? CNPJDest { get; set; }
    [XmlElement("emailDest")] public string? EmailDest { get; set; }
    [XmlElement("dhRegEvento")] public string? DhRegEvento { get; set; }
    [XmlElement("nProt")] public string? NProt { get; set; }
}

public static class TipoEvento
{
    public const string Cancelamento = "110111";
    public const string CartaCorrecao = "110110";
    public const string EpecAutorizacao = "110140";
    public const string ManifestacaoCienciaOperacao = "210210";
    public const string ManifestacaoConfirmacaoOperacao = "210200";
    public const string ManifestacaoDesconhecimentoOperacao = "210220";
    public const string ManifestacaoOperacaoNaoRealizada = "210240";
}
