namespace Acme.Sistemas.Core.Helper;

public static class CnpjHelper
{
    public static string OnlyDigits(string cnpj) => new(cnpj.Where(char.IsDigit).ToArray());

    public static bool IsValid(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj)) return false;
        var digits = OnlyDigits(cnpj);
        if (digits.Length != 14) return false;
        if (digits.Distinct().Count() == 1) return false;

        int[] mult1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] mult2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        var sum = 0;
        for (int i = 0; i < 12; i++) sum += (digits[i] - '0') * mult1[i];
        var rest = sum % 11;
        var d1 = rest < 2 ? 0 : 11 - rest;
        if ((digits[12] - '0') != d1) return false;

        sum = 0;
        for (int i = 0; i < 13; i++) sum += (digits[i] - '0') * mult2[i];
        rest = sum % 11;
        var d2 = rest < 2 ? 0 : 11 - rest;
        return (digits[13] - '0') == d2;
    }
}
