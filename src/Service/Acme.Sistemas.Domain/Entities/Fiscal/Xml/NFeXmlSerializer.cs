using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Acme.Sistemas.Domain.Entities.Fiscal.Xml;

/// <summary>
/// Serializa e deserializa NFe / NFeProc com namespaces oficiais do portal fiscal,
/// sem prefixos espúrios e com a declaração XML padrão UTF-8.
///
/// Decisões de output:
/// - Encoding UTF-8 sem BOM (a SEFAZ rejeita BOM no body).
/// - Sem `xsi`/`xsd` (não-úteis para NFe).
/// - Sem indentação por padrão (o XML transmitido é canonicalizado depois de assinado).
/// - O wrapper indentado é exposto via overload pra debug/golden file.
/// </summary>
public static class NFeXmlSerializer
{
    private static readonly XmlSerializer NFeSerializer = new(typeof(NFe));
    private static readonly XmlSerializer NFeProcSerializer = new(typeof(NFeProc));

    private static readonly XmlSerializerNamespaces NamespacesPadrao = MakeNamespaces();

    private static XmlSerializerNamespaces MakeNamespaces()
    {
        var ns = new XmlSerializerNamespaces();
        // Default namespace: portal fiscal — sem prefixo.
        ns.Add("", NFeNamespaces.Portal);
        return ns;
    }

    public static string SerializarNFe(NFe nfe, bool indentado = false)
        => Serializar(NFeSerializer, nfe, indentado);

    public static string SerializarNFeProc(NFeProc procNFe, bool indentado = false)
        => Serializar(NFeProcSerializer, procNFe, indentado);

    public static NFe DeserializarNFe(string xml)
        => (NFe)NFeSerializer.Deserialize(new StringReader(xml))!;

    public static NFeProc DeserializarNFeProc(string xml)
        => (NFeProc)NFeProcSerializer.Deserialize(new StringReader(xml))!;

    private static string Serializar(XmlSerializer serializer, object obj, bool indentado)
    {
        // UTF-8 sem BOM — SEFAZ rejeita BOM no body do envelope SOAP.
        var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
        using var ms = new MemoryStream();
        var settings = new XmlWriterSettings
        {
            Encoding = encoding,
            Indent = indentado,
            OmitXmlDeclaration = false,
        };
        using (var writer = XmlWriter.Create(ms, settings))
        {
            serializer.Serialize(writer, obj, NamespacesPadrao);
        }
        return encoding.GetString(ms.ToArray());
    }
}
