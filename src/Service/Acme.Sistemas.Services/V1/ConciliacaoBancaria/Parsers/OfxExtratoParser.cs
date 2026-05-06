using System.Globalization;
using System.Text.RegularExpressions;
using Acme.Sistemas.Domain.Entities.Financeiro;

namespace Acme.Sistemas.Services.V1.ConciliacaoBancaria.Parsers;

/// <summary>
/// Parser OFX/SGML simplificado — extrai blocos &lt;STMTTRN&gt;...&lt;/STMTTRN&gt;
/// e seus campos TRNTYPE, DTPOSTED, TRNAMT, MEMO/CHECKNUM.
/// </summary>
public sealed class OfxExtratoParser : IExtratoParser
{
    public string Formato => "OFX";

    private static readonly Regex BlockRegex =
        new(@"<STMTTRN>(?<body>.*?)</STMTTRN>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

    public IReadOnlyList<ParsedExtratoItem> Parse(Stream content)
    {
        using var reader = new StreamReader(content);
        var text = reader.ReadToEnd();

        var items = new List<ParsedExtratoItem>();
        foreach (Match match in BlockRegex.Matches(text))
        {
            var body = match.Groups["body"].Value;
            var trnAmt = ExtractTag(body, "TRNAMT");
            var dtPosted = ExtractTag(body, "DTPOSTED");
            var memo = ExtractTag(body, "MEMO");
            var checkNum = ExtractTag(body, "CHECKNUM") ?? ExtractTag(body, "FITID");

            if (!decimal.TryParse(trnAmt, NumberStyles.Any, CultureInfo.InvariantCulture, out var valor))
                continue;
            if (!TryParseOfxDate(dtPosted, out var data))
                continue;

            items.Add(new ParsedExtratoItem(
                data,
                Math.Abs(valor),
                valor < 0 ? TipoMovimentoExtrato.Debito : TipoMovimentoExtrato.Credito,
                memo,
                checkNum));
        }

        return items;
    }

    private static string? ExtractTag(string body, string tag)
    {
        var pattern = $@"<{tag}>([^<\r\n]*)";
        var match = Regex.Match(body, pattern, RegexOptions.IgnoreCase);
        return match.Success ? match.Groups[1].Value.Trim() : null;
    }

    private static bool TryParseOfxDate(string? raw, out DateTime data)
    {
        data = default;
        if (string.IsNullOrWhiteSpace(raw)) return false;
        var s = raw.Length >= 8 ? raw[..8] : raw;
        return DateTime.TryParseExact(s, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out data);
    }
}
