using Acme.Sistemas.Domain.Entities.Fiscal.Xml.Servicos;
using Acme.Sistemas.ExternalIntegration.Sefaz.Servicos;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class NFeAutorizacaoServiceTests
{
    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "NFeAutorizacaoService")]
    [Fact(DisplayName = "Dado retEnviNFe síncrono com cStat=100 (autorizado), quando ParseRetorno, então retorna Autorizado=true com protocolo e chave")]
    public void ParseRetorno_AutorizadoSincrono_RetornaProtocolo()
    {
        var xml = """
            <retEnviNFe versao="4.00" xmlns="http://www.portalfiscal.inf.br/nfe">
              <tpAmb>2</tpAmb>
              <verAplic>SP_NFE_PL009p</verAplic>
              <cStat>104</cStat>
              <xMotivo>Lote processado</xMotivo>
              <protNFe versao="4.00">
                <infProt>
                  <tpAmb>2</tpAmb>
                  <verAplic>SP_NFE_PL009p</verAplic>
                  <chNFe>35260512345678000199550010000000007111908850</chNFe>
                  <dhRecbto>2026-05-08T10:30:00-03:00</dhRecbto>
                  <nProt>135260000000001</nProt>
                  <digVal>abc=</digVal>
                  <cStat>100</cStat>
                  <xMotivo>Autorizado o uso da NF-e</xMotivo>
                </infProt>
              </protNFe>
            </retEnviNFe>
            """;

        var resultado = NFeAutorizacaoService.ParseRetorno(xml);

        resultado.Autorizado.Should().BeTrue();
        resultado.CStat.Should().Be("100");
        resultado.Protocolo.Should().Be("135260000000001");
        resultado.ChaveAcesso.Should().Be("35260512345678000199550010000000007111908850");
        resultado.NRecibo.Should().BeNull();
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "NFeAutorizacaoService")]
    [Fact(DisplayName = "Dado retEnviNFe assíncrono com cStat=103 (lote recebido), quando ParseRetorno, então retorna NRecibo e Autorizado=false")]
    public void ParseRetorno_LoteRecebidoAssincrono_RetornaNRecibo()
    {
        var xml = """
            <retEnviNFe versao="4.00" xmlns="http://www.portalfiscal.inf.br/nfe">
              <tpAmb>2</tpAmb>
              <verAplic>SP_NFE_PL009p</verAplic>
              <cStat>103</cStat>
              <xMotivo>Lote recebido com sucesso</xMotivo>
              <infRec>
                <nRec>351000000000001</nRec>
                <tMed>2</tMed>
              </infRec>
            </retEnviNFe>
            """;

        var resultado = NFeAutorizacaoService.ParseRetorno(xml);

        resultado.Autorizado.Should().BeFalse();
        resultado.CStat.Should().Be("103");
        resultado.NRecibo.Should().Be("351000000000001");
        resultado.Protocolo.Should().BeNull();
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "NFeAutorizacaoService")]
    [Fact(DisplayName = "Dado retEnviNFe com cStat=225 (assinatura inválida) sem protNFe nem infRec, quando ParseRetorno, então retorna Autorizado=false com motivo")]
    public void ParseRetorno_ErroDeLote_RetornaCStatComMotivo()
    {
        var xml = """
            <retEnviNFe versao="4.00" xmlns="http://www.portalfiscal.inf.br/nfe">
              <tpAmb>2</tpAmb>
              <verAplic>SP_NFE_PL009p</verAplic>
              <cStat>225</cStat>
              <xMotivo>Falha no Schema XML do lote de NFe</xMotivo>
            </retEnviNFe>
            """;

        var resultado = NFeAutorizacaoService.ParseRetorno(xml);

        resultado.Autorizado.Should().BeFalse();
        resultado.CStat.Should().Be("225");
        resultado.XMotivo.Should().Contain("Falha no Schema");
        resultado.Protocolo.Should().BeNull();
        resultado.NRecibo.Should().BeNull();
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "SefazResultadoCodigo")]
    [Theory(DisplayName = "Dado um cStat conhecido, quando IsAutorizado, então retorna true para 100/135/136 e false para os demais")]
    [InlineData("100", true)]
    [InlineData("135", true)]
    [InlineData("136", true)]
    [InlineData("103", false)]
    [InlineData("104", false)]
    [InlineData("204", false)]
    [InlineData("225", false)]
    [InlineData(null, false)]
    public void IsAutorizado_RetornaTrueSomenteParaSucesso(string? cStat, bool esperado)
    {
        SefazResultadoCodigo.IsAutorizado(cStat).Should().Be(esperado);
    }

    [Trait("Solucao", "Domain")]
    [Trait("Acao", "SefazResultadoCodigo")]
    [Theory(DisplayName = "Dado um cStat de paralisação (108/109), quando IsParalisacao, então retorna true; caso contrário false")]
    [InlineData("108", true)]
    [InlineData("109", true)]
    [InlineData("107", false)]
    [InlineData("100", false)]
    public void IsParalisacao_DetectaApenasCodigosDeParalisacao(string cStat, bool esperado)
    {
        SefazResultadoCodigo.IsParalisacao(cStat).Should().Be(esperado);
    }
}
