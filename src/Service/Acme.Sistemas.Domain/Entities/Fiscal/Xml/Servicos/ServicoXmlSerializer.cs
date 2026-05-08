using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Acme.Sistemas.Domain.Entities.Fiscal.Xml.Servicos;

/// <summary>
/// Serializa/deserializa payloads dos serviços SEFAZ (enviNFe, consSitNFe, envEvento, inutNFe e retornos).
/// UTF-8 sem BOM, namespace portal fiscal sem prefixo.
/// </summary>
public static class ServicoXmlSerializer
{
    private static readonly XmlSerializerNamespaces Ns = MakeNs();

    private static XmlSerializerNamespaces MakeNs()
    {
        var ns = new XmlSerializerNamespaces();
        ns.Add("", NFeNamespaces.Portal);
        return ns;
    }

    public static string Serializar<T>(T payload) where T : notnull
    {
        var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
        var settings = new XmlWriterSettings
        {
            Encoding = encoding,
            OmitXmlDeclaration = false,
            Indent = false,
        };
        using var ms = new MemoryStream();
        using (var writer = XmlWriter.Create(ms, settings))
        {
            new XmlSerializer(typeof(T)).Serialize(writer, payload, Ns);
        }
        return encoding.GetString(ms.ToArray());
    }

    public static T Deserializar<T>(string xml) where T : notnull
    {
        // Tolera: payload com declaração XML, sem ela, com whitespace, ou wrapper extra.
        using var reader = new StringReader(xml);
        return (T)new XmlSerializer(typeof(T)).Deserialize(reader)!;
    }
}
