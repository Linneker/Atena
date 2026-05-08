using Acme.Sistemas.Domain.Entities.Fiscal.Xml;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class NFeXmlSerializerTests
{
    private static NFe SampleNFe()
    {
        return new NFe
        {
            InfNFe = new InfNFe
            {
                Id = "NFe35260512345678000199550010000000007111908850" + ChaveAcessoBuilder.CalcularDV("3526051234567800019955001000000007111908850"),
                Versao = "4.00",
                Ide = new Ide
                {
                    CUF = "35", CNF = "11908850", NatOp = "VENDA",
                    Mod = "55", Serie = "1", NNF = "7",
                    DhEmi = "2026-05-08T10:30:00-03:00",
                    TpNF = TpNF.Saida, IdDest = IdDest.OperacaoInterna,
                    CMunFG = "3550308", TpImp = TpImp.DanfeRetrato, TpEmis = TpEmis.Normal,
                    CDV = "0",
                    TpAmb = TpAmb.Homologacao, FinNFe = FinNFe.Normal,
                    IndFinal = IndFinal.Sim, IndPres = IndPres.OperacaoPresencial,
                    ProcEmi = ProcEmi.Aplicativo, VerProc = "Atena 1.0",
                },
                Emit = new Emit
                {
                    CNPJ = "12345678000199", XNome = "EMITENTE TESTE LTDA",
                    EnderEmit = new Endereco
                    {
                        XLgr = "RUA TESTE", Nro = "100", XBairro = "CENTRO",
                        CMun = "3550308", XMun = "SAO PAULO", UF = "SP", CEP = "01000000",
                    },
                    IE = "111111111111", CRT = CRT.SimplesNacional,
                },
                Det =
                {
                    new Det
                    {
                        NItem = "1",
                        Prod = new Prod
                        {
                            CProd = "P001", XProd = "PRODUTO TESTE", NCM = "00000000",
                            CFOP = "5102", UCom = "UN", QCom = "1.0000",
                            VUnCom = "10.00", VProd = "10.00",
                            UTrib = "UN", QTrib = "1.0000", VUnTrib = "10.00",
                        },
                        Imposto = new Imposto
                        {
                            ICMS = new ICMS { ICMSSN102 = new ICMSSN102() },
                            PIS = new PIS { PISNT = new PISNT() },
                            COFINS = new COFINS { COFINSNT = new COFINSNT() },
                        },
                    },
                },
                Total = new Total { ICMSTot = new ICMSTot { VProd = "10.00", VNF = "10.00" } },
                Transp = new Transp { ModFrete = ModFrete.SemFrete },
                Pag = new Pag { DetPag = { new DetPag { TPag = TpPag.Dinheiro, VPag = "10.00" } } },
            },
        };
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "NFeXmlSerializer")]
    [Fact(DisplayName = "Dado uma NFe sample, quando SerializarNFe, então gera XML UTF-8 com namespace portal fiscal e elementos esperados na ordem")]
    public void SerializarNFe_GeraXmlComNamespaceCorreto()
    {
        var nfe = SampleNFe();

        var xml = NFeXmlSerializer.SerializarNFe(nfe);

        xml.Should().Contain("encoding=\"utf-8\"");
        xml.Should().Contain($"xmlns=\"{NFeNamespaces.Portal}\"");
        xml.Should().Contain("<NFe");
        xml.Should().Contain("<infNFe");
        xml.Should().Contain("<ide>");
        xml.Should().Contain("<emit>");
        xml.Should().Contain("<det nItem=\"1\">");
        xml.Should().Contain("<total>");
        xml.Should().Contain("<pag>");
        // Sem BOM (primeiro byte deve ser '<')
        xml[0].Should().Be('<');
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "NFeXmlSerializer")]
    [Fact(DisplayName = "Dado XML serializado, quando deserializa de volta e re-serializa, então o segundo XML é equivalente ao primeiro (round-trip)")]
    public void RoundTrip_DeserializeESerializeNovamente_GeraXmlEquivalente()
    {
        var nfe = SampleNFe();

        var xml1 = NFeXmlSerializer.SerializarNFe(nfe);
        var nfe2 = NFeXmlSerializer.DeserializarNFe(xml1);
        var xml2 = NFeXmlSerializer.SerializarNFe(nfe2);

        xml2.Should().Be(xml1);
    }
}
