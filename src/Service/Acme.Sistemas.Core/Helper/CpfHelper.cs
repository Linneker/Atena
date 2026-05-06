namespace Acme.Sistemas.Core.Helper;

public static class CpfHelper
{
    public static string OnlyDigits(string cpf) => new(cpf.Where(char.IsDigit).ToArray());

    public static bool IsValid(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf)) return false;
        var digits = OnlyDigits(cpf);
        if (digits.Length != 11) return false;
        if (digits.Distinct().Count() == 1) return false;

        var sum = 0;
        for (int i = 0; i < 9; i++) sum += (digits[i] - '0') * (10 - i);
        var rest = sum % 11;
        var d1 = rest < 2 ? 0 : 11 - rest;
        if ((digits[9] - '0') != d1) return false;

        sum = 0;
        for (int i = 0; i < 10; i++) sum += (digits[i] - '0') * (11 - i);
        rest = sum % 11;
        var d2 = rest < 2 ? 0 : 11 - rest;
        return (digits[10] - '0') == d2;
    }
}
