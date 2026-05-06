using System.Globalization;
using Acme.Sistemas.Domain.Entities.Financeiro;

namespace Acme.Sistemas.Services.V1.ConciliacaoBancaria.Parsers;

/// <summary>
/// CSV simples no formato: data;valor;descricao;documento
/// Suporta valor com sinal negativo (débito) ou positivo (crédito).
/// Datas em dd/MM/yyyy ou yyyy-MM-dd. Separador padrão: ponto-e-vírgula.
/// </summary>
public sealed class CsvExtratoParser : IExtratoParser
{
    public string Formato => "CSV";

    public IReadOnlyList<ParsedExtratoItem> Parse(Stream content)
    {
        var items = new List<ParsedExtratoItem>();
        using var reader = new StreamReader(content);

        string? line;
        var lineNumber = 0;
        while ((line = reader.ReadLine()) is not null)
        {
            lineNumber++;
            if (string.IsNullOrWhiteSpace(line)) continue;

            // pula header se a primeira coluna não parecer data
            if (lineNumber == 1 && !LooksLikeDate(line.Split(';', ',')[0])) continue;

            var sep = line.Contains(';') ? ';' : ',';
            var cols = line.Split(sep);
            if (cols.Length < 2) continue;

            if (!TryParseDate(cols[0], out var data)) continue;
            if (!decimal.TryParse(cols[1].Trim().Replace(".", ""), NumberStyles.Any, new CultureInfo("pt-BR"), out var valor)
                && !decimal.TryParse(cols[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out valor))
                continue;

            var tipo = valor < 0 ? TipoMovimentoExtrato.Debito : TipoMovimentoExtrato.Credito;
            items.Add(new ParsedExtratoItem(
                data,
                Math.Abs(valor),
                tipo,
                cols.Length > 2 ? cols[2].Trim() : null,
                cols.Length > 3 ? cols[3].Trim() : null));
        }

        return items;
    }

    private static bool LooksLikeDate(string s) => TryParseDate(s, out _);

    private static bool TryParseDate(string s, out DateTime data)
    {
        s = s.Trim().Trim('"');
        return DateTime.TryParseExact(s, new[] { "dd/MM/yyyy", "yyyy-MM-dd", "dd-MM-yyyy" },
            CultureInfo.InvariantCulture, DateTimeStyles.None, out data);
    }
}
