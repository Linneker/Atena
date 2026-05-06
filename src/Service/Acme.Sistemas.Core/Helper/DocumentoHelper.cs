namespace Acme.Sistemas.Core.Helper;

public static class DocumentoHelper
{
    /// <summary>Valida CPF (11 dígitos) ou CNPJ (14 dígitos) automaticamente.</summary>
    public static bool IsValid(string documento)
    {
        if (string.IsNullOrWhiteSpace(documento)) return false;
        var digits = new string(documento.Where(char.IsDigit).ToArray());
        return digits.Length switch
        {
            11 => CpfHelper.IsValid(digits),
            14 => CnpjHelper.IsValid(digits),
            _ => false
        };
    }

    public static string OnlyDigits(string documento) =>
        new(documento.Where(char.IsDigit).ToArray());
}
