namespace Acme.Sistemas.Core.Helper;

/// <summary>
/// Validação local de chave de acesso de NF-e (44 dígitos).
/// Estrutura: cUF(2) + AAMM(4) + CNPJ(14) + mod(2) + serie(3) + nNF(9) + tpEmis(1) + cNF(8) + cDV(1).
/// </summary>
public static class NFeChaveAcessoHelper
{
    public static string OnlyDigits(string chave) => new(chave.Where(char.IsDigit).ToArray());

    public static bool IsValid(string chave)
    {
        if (string.IsNullOrWhiteSpace(chave)) return false;
        var digits = OnlyDigits(chave);
        if (digits.Length != 44) return false;

        var cDvInformado = digits[43] - '0';
        var cDvCalculado = CalcularDV(digits[..43]);
        return cDvInformado == cDvCalculado;
    }

    private static int CalcularDV(string chave43)
    {
        // Módulo 11 com pesos 2..9 ciclando da direita para esquerda
        int sum = 0;
        int peso = 2;
        for (int i = chave43.Length - 1; i >= 0; i--)
        {
            sum += (chave43[i] - '0') * peso;
            peso = peso == 9 ? 2 : peso + 1;
        }
        var resto = sum % 11;
        var dv = 11 - resto;
        return dv >= 10 ? 0 : dv;
    }
}
