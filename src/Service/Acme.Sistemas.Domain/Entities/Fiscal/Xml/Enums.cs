using System.Xml.Serialization;

namespace Acme.Sistemas.Domain.Entities.Fiscal.Xml;

public enum TpAmb
{
    [XmlEnum("1")] Producao = 1,
    [XmlEnum("2")] Homologacao = 2,
}

public enum TpNF
{
    [XmlEnum("0")] Entrada = 0,
    [XmlEnum("1")] Saida = 1,
}

public enum IdDest
{
    [XmlEnum("1")] OperacaoInterna = 1,
    [XmlEnum("2")] OperacaoInterestadual = 2,
    [XmlEnum("3")] OperacaoExterior = 3,
}

public enum TpImp
{
    [XmlEnum("0")] SemDanfe = 0,
    [XmlEnum("1")] DanfeRetrato = 1,
    [XmlEnum("2")] DanfePaisagem = 2,
    [XmlEnum("3")] DanfeSimplificado = 3,
    [XmlEnum("4")] DanfeNFCe = 4,
    [XmlEnum("5")] DanfeNFCeMsgEletronica = 5,
}

public enum TpEmis
{
    [XmlEnum("1")] Normal = 1,
    [XmlEnum("2")] ContingenciaFS = 2,
    [XmlEnum("3")] ContingenciaSCAN = 3,
    [XmlEnum("4")] ContingenciaEPEC = 4,
    [XmlEnum("5")] ContingenciaFSDA = 5,
    [XmlEnum("6")] ContingenciaSVCAN = 6,
    [XmlEnum("7")] ContingenciaSVCRS = 7,
    [XmlEnum("9")] ContingenciaOffLineNFCe = 9,
}

public enum FinNFe
{
    [XmlEnum("1")] Normal = 1,
    [XmlEnum("2")] Complementar = 2,
    [XmlEnum("3")] Ajuste = 3,
    [XmlEnum("4")] Devolucao = 4,
}

public enum IndFinal
{
    [XmlEnum("0")] Nao = 0,
    [XmlEnum("1")] Sim = 1,
}

public enum IndPres
{
    [XmlEnum("0")] NaoSeAplica = 0,
    [XmlEnum("1")] OperacaoPresencial = 1,
    [XmlEnum("2")] OperacaoNaoPresencialInternet = 2,
    [XmlEnum("3")] OperacaoNaoPresencialTeleatendimento = 3,
    [XmlEnum("4")] NfceEntregaDomicilio = 4,
    [XmlEnum("5")] OperacaoPresencialForaEstabelecimento = 5,
    [XmlEnum("9")] OperacaoNaoPresencialOutros = 9,
}

public enum ProcEmi
{
    [XmlEnum("0")] Aplicativo = 0,
    [XmlEnum("1")] AvulsaFisco = 1,
    [XmlEnum("2")] AvulsaContribuinte = 2,
    [XmlEnum("3")] PelopProprio = 3,
}

public enum CRT
{
    [XmlEnum("1")] SimplesNacional = 1,
    [XmlEnum("2")] SimplesNacionalExcessoLimite = 2,
    [XmlEnum("3")] RegimeNormal = 3,
    [XmlEnum("4")] SimplesNacionalMEI = 4,
}

public enum IndIEDest
{
    [XmlEnum("1")] ContribuinteICMS = 1,
    [XmlEnum("2")] ContribuinteIsento = 2,
    [XmlEnum("9")] NaoContribuinte = 9,
}

public enum ModFrete
{
    [XmlEnum("0")] PorContaEmitente = 0,
    [XmlEnum("1")] PorContaDestinatario = 1,
    [XmlEnum("2")] PorContaTerceiros = 2,
    [XmlEnum("3")] TransporteProprioRemetente = 3,
    [XmlEnum("4")] TransporteProprioDestinatario = 4,
    [XmlEnum("9")] SemFrete = 9,
}

public enum TpPag
{
    [XmlEnum("01")] Dinheiro = 1,
    [XmlEnum("02")] Cheque = 2,
    [XmlEnum("03")] CartaoCredito = 3,
    [XmlEnum("04")] CartaoDebito = 4,
    [XmlEnum("05")] CreditoLoja = 5,
    [XmlEnum("10")] ValeAlimentacao = 10,
    [XmlEnum("11")] ValeRefeicao = 11,
    [XmlEnum("12")] ValePresente = 12,
    [XmlEnum("13")] ValeCombustivel = 13,
    [XmlEnum("15")] BoletoBancario = 15,
    [XmlEnum("16")] DepositoBancario = 16,
    [XmlEnum("17")] PagamentoInstantaneoPIX = 17,
    [XmlEnum("18")] TransferenciaBancaria = 18,
    [XmlEnum("19")] ProgramaFidelidade = 19,
    [XmlEnum("90")] SemPagamento = 90,
    [XmlEnum("99")] Outros = 99,
}
