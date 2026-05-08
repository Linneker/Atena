using System.Text;
using System.Xml;

namespace Acme.Sistemas.ExternalIntegration.Sefaz.Soap;

/// <summary>
/// Monta envelope SOAP 1.2 para webservices SEFAZ NF-e v4.00.
///
/// Estrutura padrão:
/// <code>
/// &lt;soap12:Envelope xmlns:soap12="http://www.w3.org/2003/05/soap-envelope"&gt;
///   &lt;soap12:Body&gt;
///     &lt;nfeDadosMsg xmlns="http://www.portalfiscal.inf.br/nfe/wsdl/{servico}"&gt;
///       {payload XML}
///     &lt;/nfeDadosMsg&gt;
///   &lt;/soap12:Body&gt;
/// &lt;/soap12:Envelope&gt;
/// </code>
///
/// O Content-Type da requisição HTTP deve ser
/// <c>application/soap+xml; charset=utf-8; action="..."</c> com a SOAP Action correspondente.
/// </summary>
public static class SoapEnvelopeBuilder
{
    public const string SoapEnvelopeNs = "http://www.w3.org/2003/05/soap-envelope";

    /// <summary>
    /// Monta o envelope SOAP em volta de um payload XML já assinado.
    /// </summary>
    /// <param name="payloadXml">XML do `enviNFe`, `consSitNFe`, `envEvento`, etc. (já assinado).</param>
    /// <param name="wsdlNamespace">Namespace WSDL do serviço (ex.: "http://www.portalfiscal.inf.br/nfe/wsdl/NFeAutorizacao4").</param>
    public static string Build(string payloadXml, string wsdlNamespace)
    {
        if (string.IsNullOrWhiteSpace(payloadXml))
            throw new ArgumentException("Payload vazio.", nameof(payloadXml));
        if (string.IsNullOrWhiteSpace(wsdlNamespace))
            throw new ArgumentException("Namespace WSDL é obrigatório.", nameof(wsdlNamespace));

        // Parse do payload pra normalizar (remover declaração XML aninhada).
        var doc = new XmlDocument { PreserveWhitespace = true };
        doc.LoadXml(payloadXml);
        var payloadOuter = doc.DocumentElement?.OuterXml
            ?? throw new InvalidOperationException("Payload sem elemento raiz.");

        var sb = new StringBuilder();
        sb.Append("""<?xml version="1.0" encoding="utf-8"?>""");
        sb.Append($"""<soap12:Envelope xmlns:soap12="{SoapEnvelopeNs}">""");
        sb.Append("<soap12:Body>");
        sb.Append($"""<nfeDadosMsg xmlns="{wsdlNamespace}">""");
        sb.Append(payloadOuter);
        sb.Append("</nfeDadosMsg>");
        sb.Append("</soap12:Body>");
        sb.Append("</soap12:Envelope>");
        return sb.ToString();
    }

    /// <summary>
    /// Extrai o payload `nfeResultMsg` do envelope SOAP de resposta.
    /// </summary>
    /// <returns>O conteúdo XML interno do nfeResultMsg, ou null se não encontrado.</returns>
    public static string? ExtractResultMsg(string responseSoap)
    {
        if (string.IsNullOrWhiteSpace(responseSoap)) return null;

        var doc = new XmlDocument { PreserveWhitespace = true };
        doc.LoadXml(responseSoap);

        // Busca por local-name "nfeResultMsg" em qualquer namespace
        var resultMsg = doc.GetElementsByTagName("nfeResultMsg");
        if (resultMsg.Count == 0)
        {
            // Algumas SEFAZ envelopam diferente; tenta SOAP Fault para mensagem útil
            var fault = doc.GetElementsByTagName("Fault", SoapEnvelopeNs);
            if (fault.Count > 0) return fault[0]?.OuterXml;
            return null;
        }

        return resultMsg[0]?.InnerXml;
    }
}
