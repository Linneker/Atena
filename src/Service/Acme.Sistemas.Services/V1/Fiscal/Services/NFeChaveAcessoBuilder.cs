using Acme.Sistemas.Core.Helper;

namespace Acme.Sistemas.Services.V1.Fiscal.Services;

/// <summary>
/// Constrói a chave de acesso de 44 dígitos da NF-e:
/// cUF(2) + AAMM(4) + CNPJ(14) + mod(2) + serie(3) + nNF(9) + tpEmis(1) + cNF(8) + cDV(1).
/// </summary>
public static class NFeChaveAcessoBuilder
{
    public static string Build(int codigoUf, DateTime dataEmissao, string cnpj, int serie, int numero, int tipoEmissao, int codigoNumerico)
    {
        var cnpjDigits = new string(cnpj.Where(char.IsDigit).ToArray()).PadLeft(14, '0');
        var raw = $"{codigoUf:D2}{dataEmissao:yyMM}{cnpjDigits}55{serie:D3}{numero:D9}{tipoEmissao:D1}{codigoNumerico:D8}";
        var dv = CalcularDV(raw);
        return raw + dv;
    }

    private static int CalcularDV(string s)
    {
        int sum = 0, peso = 2;
        for (int i = s.Length - 1; i >= 0; i--)
        {
            sum += (s[i] - '0') * peso;
            peso = peso == 9 ? 2 : peso + 1;
        }
        var resto = sum % 11;
        var dv = 11 - resto;
        return dv >= 10 ? 0 : dv;
    }

    /// <summary>Mapa de UF para código IBGE (cUF).</summary>
    public static int CodigoUf(string uf) => uf.ToUpperInvariant() switch
    {
        "AC" => 12, "AL" => 27, "AP" => 16, "AM" => 13, "BA" => 29, "CE" => 23, "DF" => 53,
        "ES" => 32, "GO" => 52, "MA" => 21, "MT" => 51, "MS" => 50, "MG" => 31, "PA" => 15,
        "PB" => 25, "PR" => 41, "PE" => 26, "PI" => 22, "RJ" => 33, "RN" => 24, "RS" => 43,
        "RO" => 11, "RR" => 14, "SC" => 42, "SP" => 35, "SE" => 28, "TO" => 17,
        _ => 35
    };
}
