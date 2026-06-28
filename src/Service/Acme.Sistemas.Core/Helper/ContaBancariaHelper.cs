namespace Acme.Sistemas.Core.Helper;

/// <summary>
/// Validador genérico de conta bancária brasileira: banco (3 dígitos), agência (3-5 dígitos
/// + DV opcional), conta (4-12 dígitos + DV 1 caractere alfanumérico para X de dígito).
/// Cada banco tem regras próprias de DV (Itaú mod 10, Bradesco mod 11, etc.); aqui validamos
/// apenas formato, deixando o DV específico para integração futura por banco.
/// </summary>
public static class ContaBancariaHelper
{
    public static bool IsCodigoBancoValido(string? codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo)) return false;
        var digits = new string(codigo.Where(char.IsDigit).ToArray());
        return digits.Length == 3 && digits != "000";
    }

    public static bool IsAgenciaValida(string? agencia, string? digito)
    {
        if (string.IsNullOrWhiteSpace(agencia)) return false;
        var d = new string(agencia.Where(char.IsDigit).ToArray());
        if (d.Length is < 3 or > 5) return false;
        // DV opcional, 1 caractere alfanumérico
        if (!string.IsNullOrWhiteSpace(digito) && digito.Length != 1) return false;
        return true;
    }

    public static bool IsContaValida(string? conta, string? digito)
    {
        if (string.IsNullOrWhiteSpace(conta)) return false;
        var d = new string(conta.Where(char.IsDigit).ToArray());
        if (d.Length is < 4 or > 12) return false;
        if (string.IsNullOrWhiteSpace(digito) || digito.Length != 1) return false;
        // DV pode ser dígito ou 'X' (alguns bancos usam X para 10)
        return char.IsDigit(digito[0]) || char.ToUpperInvariant(digito[0]) == 'X';
    }

    public static bool IsValid(
        string? codigoBanco, string? agencia, string? agenciaDigito, string? conta, string? contaDigito)
        => IsCodigoBancoValido(codigoBanco)
           && IsAgenciaValida(agencia, agenciaDigito)
           && IsContaValida(conta, contaDigito);
}
