using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.Domain.Entities.Fiscal.Xml.Servicos;
using Acme.Sistemas.ExternalIntegration.Sefaz.Servicos;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class EventoEInutilizacaoTests
{
    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "NFeRecepcaoEventoService")]
    [Fact(DisplayName = "Dado dados de cancelamento, quando MontarEvento(110111), então gera Evento com Id no formato ID<tpEvento><chNFe><nSeqEvento> e descEvento='Cancelamento'")]
    public void MontarEvento_Cancelamento_GeraEventoComIdCorreto()
    {
        var chave = "35260512345678000199550010000000007111908850";
        var evento = NFeRecepcaoEventoService.MontarEvento(
            chave: chave,
            cnpj: "12345678000199",
            uf: "SP",
            ambiente: AmbienteFiscal.Homologacao,
            tpEvento: TipoEvento.Cancelamento,
            descEvento: "Cancelamento",
            preencherDetalhe: det =>
            {
                det.NProt = "135260000000001";
                det.XJust = "Cancelamento por erro de digitacao no destinatario.";
            });

        evento.InfEvento.Id.Should().Be($"ID110111{chave}01");
        evento.InfEvento.TpEvento.Should().Be(TipoEvento.Cancelamento);
        evento.InfEvento.DetEvento.NProt.Should().Be("135260000000001");
        evento.InfEvento.DetEvento.DescEvento.Should().Be("Cancelamento");
        evento.InfEvento.COrgao.Should().Be("35"); // SP
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "NFeRecepcaoEventoService")]
    [Theory(DisplayName = "Dado UFs prioritárias, quando MontarEvento, então cOrgao mapeia para o cUF correto")]
    [InlineData("SP", "35")]
    [InlineData("RJ", "33")]
    [InlineData("MG", "31")]
    [InlineData("RS", "43")]
    [InlineData("PR", "41")]
    [InlineData("SVRS", "43")]
    [InlineData("SVAN", "91")]
    public void MontarEvento_COrgaoPorUF(string uf, string cOrgaoEsperado)
    {
        var evento = NFeRecepcaoEventoService.MontarEvento(
            chave: "35260512345678000199550010000000007111908850",
            cnpj: "12345678000199",
            uf: uf,
            ambiente: AmbienteFiscal.Homologacao,
            tpEvento: TipoEvento.CartaCorrecao,
            descEvento: "Carta de Correcao",
            preencherDetalhe: det => det.XCorrecao = "correcao de teste com pelo menos 15 chars");

        evento.InfEvento.COrgao.Should().Be(cOrgaoEsperado);
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "TipoEvento")]
    [Fact(DisplayName = "Dado constantes de TipoEvento, então cancelamento=110111 e carta de correcao=110110")]
    public void TipoEvento_ConstantesCorretas()
    {
        TipoEvento.Cancelamento.Should().Be("110111");
        TipoEvento.CartaCorrecao.Should().Be("110110");
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "NFeInutilizacaoService")]
    [Fact(DisplayName = "Dado retInutNFe com cStat=102 (homologado), quando deserialize, então InfInut.NProt expõe protocolo")]
    public void RetInutNFe_Deserialize_ExpoeProtocolo()
    {
        var xml = """
            <retInutNFe versao="4.00" xmlns="http://www.portalfiscal.inf.br/nfe">
              <infInut Id="ID35260512345678000199550010000000010000000020">
                <tpAmb>2</tpAmb>
                <verAplic>SP_NFE_PL009p</verAplic>
                <cStat>102</cStat>
                <xMotivo>Inutilizacao de numero homologado</xMotivo>
                <cUF>35</cUF>
                <ano>26</ano>
                <CNPJ>12345678000199</CNPJ>
                <mod>55</mod>
                <serie>001</serie>
                <nNFIni>000000010</nNFIni>
                <nNFFin>000000020</nNFFin>
                <dhRecbto>2026-05-08T10:30:00-03:00</dhRecbto>
                <nProt>135260000000999</nProt>
              </infInut>
            </retInutNFe>
            """;

        var ret = ServicoXmlSerializer.Deserializar<RetInutNFe>(xml);

        ret.InfInut.CStat.Should().Be("102");
        ret.InfInut.NProt.Should().Be("135260000000999");
        ret.InfInut.NNFIni.Should().Be("000000010");
        ret.InfInut.NNFFin.Should().Be("000000020");
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "ServicoXmlSerializer")]
    [Fact(DisplayName = "Dado um ConsStatServ, quando Serializar e Deserializar, então o objeto retornado preserva os campos (round-trip)")]
    public void ServicoXmlSerializer_RoundTrip_PreservaCampos()
    {
        var consulta = new ConsStatServ { TpAmb = "2", CUF = "35" };

        var xml = ServicoXmlSerializer.Serializar(consulta);
        var deserializado = ServicoXmlSerializer.Deserializar<ConsStatServ>(xml);

        deserializado.TpAmb.Should().Be("2");
        deserializado.CUF.Should().Be("35");
        deserializado.XServ.Should().Be("STATUS");
        deserializado.Versao.Should().Be("4.00");
    }
}
