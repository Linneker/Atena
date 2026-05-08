using Acme.Sistemas.ExternalIntegration.Sefaz.Urls;

namespace Acme.Sistemas.ExternalIntegration.Sefaz.Soap;

/// <summary>
/// SOAP Actions e namespaces WSDL por serviço SEFAZ NF-e v4.00.
/// Ambos vão no Content-Type (action) e como xmlns do nfeDadosMsg.
/// Referência: especificações WSDL publicadas pela Receita Federal.
/// </summary>
public static class SoapAction
{
    private const string WsdlBase = "http://www.portalfiscal.inf.br/nfe/wsdl/";

    public static (string WsdlNamespace, string Action) For(SefazServico servico) => servico switch
    {
        SefazServico.Autorizacao =>
            (WsdlBase + "NFeAutorizacao4", WsdlBase + "NFeAutorizacao4/nfeAutorizacaoLote"),
        SefazServico.RetAutorizacao =>
            (WsdlBase + "NFeRetAutorizacao4", WsdlBase + "NFeRetAutorizacao4/nfeRetAutorizacaoLote"),
        SefazServico.ConsultaProtocolo =>
            (WsdlBase + "NFeConsultaProtocolo4", WsdlBase + "NFeConsultaProtocolo4/nfeConsultaNF"),
        SefazServico.StatusServico =>
            (WsdlBase + "NFeStatusServico4", WsdlBase + "NFeStatusServico4/nfeStatusServicoNF"),
        SefazServico.RecepcaoEvento =>
            (WsdlBase + "NFeRecepcaoEvento4", WsdlBase + "NFeRecepcaoEvento4/nfeRecepcaoEvento"),
        SefazServico.Inutilizacao =>
            (WsdlBase + "NFeInutilizacao4", WsdlBase + "NFeInutilizacao4/nfeInutilizacaoNF"),
        _ => throw new ArgumentOutOfRangeException(nameof(servico)),
    };
}
