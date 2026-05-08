using System.Xml;
using Acme.Sistemas.ExternalIntegration.Sefaz.Soap;
using Acme.Sistemas.ExternalIntegration.Sefaz.Urls;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class SoapEnvelopeBuilderTests
{
    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "SoapEnvelopeBuilder")]
    [Fact(DisplayName = "Dado um payload XML, quando Build com namespace WSDL, então gera envelope SOAP 1.2 com nfeDadosMsg envolvendo o payload")]
    public void Build_PayloadValido_GeraEnvelopeBemFormado()
    {
        var payload = """<enviNFe versao="4.00" xmlns="http://www.portalfiscal.inf.br/nfe"><idLote>1</idLote></enviNFe>""";
        var ns = "http://www.portalfiscal.inf.br/nfe/wsdl/NFeAutorizacao4";

        var envelope = SoapEnvelopeBuilder.Build(payload, ns);

        envelope.Should().Contain("soap12:Envelope");
        envelope.Should().Contain("xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\"");
        envelope.Should().Contain($"<nfeDadosMsg xmlns=\"{ns}\">");
        envelope.Should().Contain("<enviNFe versao=\"4.00\"");
        envelope.Should().Contain("</nfeDadosMsg>");

        // Garantia de XML bem-formado
        var doc = new XmlDocument();
        Action parse = () => doc.LoadXml(envelope);
        parse.Should().NotThrow();
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "SoapEnvelopeBuilder")]
    [Fact(DisplayName = "Dado um payload vazio, quando Build, então lança ArgumentException")]
    public void Build_PayloadVazio_Lanca()
    {
        Action act = () => SoapEnvelopeBuilder.Build("", "http://example/wsdl");
        act.Should().Throw<ArgumentException>().WithMessage("*Payload*");
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "SoapEnvelopeBuilder")]
    [Fact(DisplayName = "Dado uma resposta SOAP com nfeResultMsg, quando ExtractResultMsg, então retorna o conteúdo interno")]
    public void ExtractResultMsg_RespostaValida_RetornaConteudoInterno()
    {
        var resposta = """
            <?xml version="1.0" encoding="utf-8"?>
            <soap:Envelope xmlns:soap="http://www.w3.org/2003/05/soap-envelope">
              <soap:Body>
                <nfeResultMsg xmlns="http://www.portalfiscal.inf.br/nfe/wsdl/NFeAutorizacao4">
                  <retEnviNFe versao="4.00" xmlns="http://www.portalfiscal.inf.br/nfe">
                    <cStat>104</cStat>
                  </retEnviNFe>
                </nfeResultMsg>
              </soap:Body>
            </soap:Envelope>
            """;

        var msg = SoapEnvelopeBuilder.ExtractResultMsg(resposta);

        msg.Should().NotBeNull();
        msg!.Should().Contain("<retEnviNFe").And.Contain("<cStat>104</cStat>");
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "SoapEnvelopeBuilder")]
    [Fact(DisplayName = "Dado uma resposta com SOAP Fault em vez de nfeResultMsg, quando ExtractResultMsg, então retorna o XML do Fault para diagnóstico")]
    public void ExtractResultMsg_ComFault_RetornaFaultParaDiagnostico()
    {
        var resposta = """
            <?xml version="1.0" encoding="utf-8"?>
            <soap:Envelope xmlns:soap="http://www.w3.org/2003/05/soap-envelope">
              <soap:Body>
                <soap:Fault>
                  <soap:Code><soap:Value>soap:Receiver</soap:Value></soap:Code>
                  <soap:Reason><soap:Text>Erro interno</soap:Text></soap:Reason>
                </soap:Fault>
              </soap:Body>
            </soap:Envelope>
            """;

        var msg = SoapEnvelopeBuilder.ExtractResultMsg(resposta);

        msg.Should().NotBeNull();
        msg!.Should().Contain("Fault").And.Contain("Erro interno");
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "SoapAction")]
    [Theory(DisplayName = "Dado um SefazServico, quando SoapAction.For, então retorna namespace WSDL e action coerentes")]
    [InlineData(SefazServico.Autorizacao, "NFeAutorizacao4", "nfeAutorizacaoLote")]
    [InlineData(SefazServico.RetAutorizacao, "NFeRetAutorizacao4", "nfeRetAutorizacaoLote")]
    [InlineData(SefazServico.ConsultaProtocolo, "NFeConsultaProtocolo4", "nfeConsultaNF")]
    [InlineData(SefazServico.StatusServico, "NFeStatusServico4", "nfeStatusServicoNF")]
    [InlineData(SefazServico.RecepcaoEvento, "NFeRecepcaoEvento4", "nfeRecepcaoEvento")]
    [InlineData(SefazServico.Inutilizacao, "NFeInutilizacao4", "nfeInutilizacaoNF")]
    public void SoapAction_For_RetornaWsdlEAction(SefazServico servico, string wsdlContains, string actionContains)
    {
        var (wsdl, action) = SoapAction.For(servico);

        wsdl.Should().StartWith("http://www.portalfiscal.inf.br/nfe/wsdl/").And.Contain(wsdlContains);
        action.Should().StartWith("http://www.portalfiscal.inf.br/nfe/wsdl/")
              .And.Contain(wsdlContains)
              .And.Contain(actionContains);
    }
}
