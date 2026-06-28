namespace Acme.Sistemas.Core.Helper;

/// <summary>
/// Validador da CTPS (Carteira de Trabalho e Previdência Social).
/// CTPS válida tem 7-8 dígitos no número + 3 dígitos na série + UF (2 letras).
/// Não há DV oficial — validação é apenas formato.
/// </summary>
public static class CtpsHelper
{
    private static readonly HashSet<string> UfsValidas = new(StringComparer.OrdinalIgnoreCase)
    {
        "AC","AL","AP","AM","BA","CE","DF","ES","GO","MA","MT","MS","MG",
        "PA","PB","PR","PE","PI","RJ","RN","RS","RO","RR","SC","SP","SE","TO"
    };

    public static bool IsValid(string? numero, string? serie, string? uf)
    {
        if (string.IsNullOrWhiteSpace(numero) || string.IsNullOrWhiteSpace(serie) || string.IsNullOrWhiteSpace(uf))
            return false;

        var numDigits = new string(numero.Where(char.IsDigit).ToArray());
        if (numDigits.Length is < 7 or > 8) return false;

        var serieDigits = new string(serie.Where(char.IsDigit).ToArray());
        if (serieDigits.Length is < 3 or > 5) return false;

        return UfsValidas.Contains(uf.Trim());
    }
}
