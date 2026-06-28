namespace Acme.Sistemas.Core.Helper;

/// <summary>
/// Validador de PIS/PASEP. Algoritmo: 11 dígitos, último é DV mod 11 com pesos 3..2.
/// PISes "zerados" (00000000000) e sequências iguais são considerados inválidos.
/// </summary>
public static class PisHelper
{
    public static string OnlyDigits(string pis) => new(pis.Where(char.IsDigit).ToArray());

    public static bool IsValid(string pis)
    {
        if (string.IsNullOrWhiteSpace(pis)) return false;
        var digits = OnlyDigits(pis);
        if (digits.Length != 11) return false;
        if (digits.Distinct().Count() == 1) return false;

        // Pesos para os 10 primeiros dígitos: 3,2,9,8,7,6,5,4,3,2
        ReadOnlySpan<int> pesos = stackalloc int[] { 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        var sum = 0;
        for (var i = 0; i < 10; i++) sum += (digits[i] - '0') * pesos[i];

        var rest = sum % 11;
        var dv = rest < 2 ? 0 : 11 - rest;
        return (digits[10] - '0') == dv;
    }
}
