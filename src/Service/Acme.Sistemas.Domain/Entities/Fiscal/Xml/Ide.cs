using System.Xml.Serialization;

namespace Acme.Sistemas.Domain.Entities.Fiscal.Xml;

/// <summary>
/// `ide` — bloco de identificação da NF-e (B01 do layout v4.00).
/// </summary>
public sealed class Ide
{
    /// <summary>Código da UF do emitente (IBGE, 2 dígitos). Ex.: 35=SP, 33=RJ.</summary>
    [XmlElement("cUF")] public string CUF { get; set; } = string.Empty;

    /// <summary>Código numérico aleatório (8 dígitos) que compõe a chave de acesso.</summary>
    [XmlElement("cNF")] public string CNF { get; set; } = string.Empty;

    /// <summary>Natureza da operação (texto descritivo: "Venda", "Devolução", etc.).</summary>
    [XmlElement("natOp")] public string NatOp { get; set; } = string.Empty;

    /// <summary>Modelo da NF-e: 55 (NFe) ou 65 (NFCe).</summary>
    [XmlElement("mod")] public string Mod { get; set; } = "55";

    [XmlElement("serie")] public string Serie { get; set; } = string.Empty;

    [XmlElement("nNF")] public string NNF { get; set; } = string.Empty;

    /// <summary>Data e hora da emissão (formato ISO com fuso, ex.: 2026-05-08T10:30:00-03:00).</summary>
    [XmlElement("dhEmi")] public string DhEmi { get; set; } = string.Empty;

    /// <summary>Data e hora da saída/entrada (opcional).</summary>
    [XmlElement("dhSaiEnt")] public string? DhSaiEnt { get; set; }

    [XmlElement("tpNF")] public TpNF TpNF { get; set; } = TpNF.Saida;

    [XmlElement("idDest")] public IdDest IdDest { get; set; } = IdDest.OperacaoInterna;

    /// <summary>Código do município de fato gerador (IBGE, 7 dígitos).</summary>
    [XmlElement("cMunFG")] public string CMunFG { get; set; } = string.Empty;

    [XmlElement("tpImp")] public TpImp TpImp { get; set; } = TpImp.DanfeRetrato;

    [XmlElement("tpEmis")] public TpEmis TpEmis { get; set; } = TpEmis.Normal;

    /// <summary>Dígito verificador da chave de acesso (último dígito dos 44).</summary>
    [XmlElement("cDV")] public string CDV { get; set; } = string.Empty;

    [XmlElement("tpAmb")] public TpAmb TpAmb { get; set; } = TpAmb.Homologacao;

    [XmlElement("finNFe")] public FinNFe FinNFe { get; set; } = FinNFe.Normal;

    [XmlElement("indFinal")] public IndFinal IndFinal { get; set; } = IndFinal.Nao;

    [XmlElement("indPres")] public IndPres IndPres { get; set; } = IndPres.OperacaoPresencial;

    [XmlElement("procEmi")] public ProcEmi ProcEmi { get; set; } = ProcEmi.Aplicativo;

    /// <summary>Versão do aplicativo emissor.</summary>
    [XmlElement("verProc")] public string VerProc { get; set; } = "Atena 1.0";

    /// <summary>Data/hora de entrada em contingência (obrigatório se tpEmis != 1).</summary>
    [XmlElement("dhCont")] public string? DhCont { get; set; }

    /// <summary>Justificativa da contingência (15-256 chars, obrigatório se tpEmis != 1).</summary>
    [XmlElement("xJust")] public string? XJust { get; set; }

    /// <summary>Documentos referenciados (NFe substituídas, devolvidas, etc.). Não modelado em profundidade.</summary>
    [XmlElement("NFref")] public List<NFref>? NFref { get; set; }
}

public sealed class NFref
{
    [XmlElement("refNFe")] public string? RefNFe { get; set; }
}
