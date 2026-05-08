using System.Collections.Concurrent;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;

namespace Acme.Sistemas.ExternalIntegration.Sefaz;

/// <summary>
/// Erro de validação XSD com posição na origem (linha, coluna).
/// </summary>
public sealed record XsdError(string Mensagem, int Linha, int Coluna, XmlSeverityType Severidade);

/// <summary>
/// Resultado da validação: lista de erros (vazia = válido).
/// </summary>
public sealed record XsdValidationResult(IReadOnlyList<XsdError> Erros)
{
    public bool Valido => Erros.Count == 0;

    public static XsdValidationResult OK { get; } = new(Array.Empty<XsdError>());
}

/// <summary>
/// Valida XML NF-e contra o schema XSD oficial v4.00.
///
/// Schemas são embutidos como <c>EmbeddedResource</c> em
/// <c>Sefaz/Schemas/v4.00/*.xsd</c>; o reader principal é cacheado para evitar reparse a cada validação.
///
/// Por padrão a Receita Federal mantém schemas separados que se importam mutuamente
/// (e.g., <c>nfe_v4.00.xsd</c> importa <c>tiposBasico_v4.00.xsd</c> e <c>xmldsig-core-schema</c>).
/// Para resolver isso, todos os XSDs da pasta são adicionados ao mesmo
/// <see cref="XmlSchemaSet"/>; imports relativos resolvem dentro do set.
/// </summary>
public sealed class XsdValidator
{
    private const string SchemaResourcePrefix = "Acme.Sistemas.ExternalIntegration.Sefaz.Schemas.v4_00.";
    private const string SchemaFolderRelative = "Sefaz/Schemas/v4.00";
    private const string TargetNamespace = "http://www.portalfiscal.inf.br/nfe";

    private static readonly Lazy<XmlSchemaSet?> _schemaSet = new(LoadSchemaSet);
    private static readonly ConcurrentDictionary<string, XmlReaderSettings> _settingsCache = new();

    /// <summary>
    /// Valida o XML contra o XSD raiz do NF-e v4.00.
    /// Retorna <see cref="XsdValidationResult.OK"/> ou lista estruturada de erros.
    /// Lança <see cref="InvalidOperationException"/> se os schemas embutidos não foram encontrados.
    /// </summary>
    public XsdValidationResult Validar(string xml)
    {
        var schemas = _schemaSet.Value
            ?? throw new InvalidOperationException(
                $"Schemas XSD NFe v4.00 não encontrados. Adicione os XSDs em {SchemaFolderRelative}/ — ver README.md naquela pasta.");

        var erros = new List<XsdError>();
        var settings = new XmlReaderSettings
        {
            ValidationType = ValidationType.Schema,
            Schemas = schemas,
            ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings,
        };
        settings.ValidationEventHandler += (_, e) =>
        {
            erros.Add(new XsdError(
                e.Message,
                e.Exception?.LineNumber ?? 0,
                e.Exception?.LinePosition ?? 0,
                e.Severity));
        };

        using var stringReader = new StringReader(xml);
        using var reader = XmlReader.Create(stringReader, settings);
        try
        {
            while (reader.Read()) { /* drain */ }
        }
        catch (XmlException ex)
        {
            erros.Add(new XsdError($"XML mal-formado: {ex.Message}", ex.LineNumber, ex.LinePosition, XmlSeverityType.Error));
        }

        return new XsdValidationResult(erros);
    }

    /// <summary>
    /// Diagnóstico — retorna true se há ao menos um schema embutido carregado.
    /// Útil em start-up para falhar rápido.
    /// </summary>
    public static bool TemSchemasCarregados() => _schemaSet.Value is { Count: > 0 };

    private static XmlSchemaSet? LoadSchemaSet()
    {
        var set = new XmlSchemaSet();
        var asm = typeof(XsdValidator).Assembly;
        var resources = asm.GetManifestResourceNames()
            .Where(r => r.StartsWith(SchemaResourcePrefix, StringComparison.Ordinal)
                     && r.EndsWith(".xsd", StringComparison.Ordinal))
            .ToList();

        if (resources.Count == 0)
            return null;

        foreach (var resource in resources)
        {
            using var stream = asm.GetManifestResourceStream(resource)!;
            using var reader = XmlReader.Create(stream);
            // targetNamespace é lido do próprio XSD; passar null deixa o XmlSchemaSet inferir.
            set.Add(null, reader);
        }

        try { set.Compile(); } catch { /* falha de compile é exposta na primeira validação */ }
        return set;
    }
}
