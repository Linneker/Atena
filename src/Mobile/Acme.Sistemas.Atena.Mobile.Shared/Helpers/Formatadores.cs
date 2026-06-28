namespace Acme.Sistemas.Atena.Mobile.Shared.Helpers;

public static class Formatadores
{
    /// <summary>Formata "HHhMM" a partir de minutos. 0 vira "—". Negativo é abs.</summary>
    public static string MinutosParaHoras(int m)
    {
        if (m == 0) return "—";
        var abs = Math.Abs(m);
        return $"{abs / 60:00}h{abs % 60:00}";
    }

    /// <summary>Mesmo que MinutosParaHoras mas inclui sinal "+" ou "-".</summary>
    public static string MinutosParaHorasComSinal(int m)
    {
        if (m == 0) return "0h00";
        var fmt = MinutosParaHoras(m);
        return m > 0 ? "+" + fmt : "-" + fmt;
    }

    /// <summary>Formata CPF 11 dígitos para XXX.XXX.XXX-XX.</summary>
    public static string FormatarCpf(string cpf)
    {
        var d = new string(cpf.Where(char.IsDigit).ToArray());
        return d.Length == 11 ? $"{d[..3]}.{d[3..6]}.{d[6..9]}-{d[9..]}" : cpf;
    }

    /// <summary>DateOnly → "dd/MM/yyyy".</summary>
    public static string FormatarData(DateOnly d) => d.ToString("dd/MM/yyyy");

    /// <summary>DateTime → "dd/MM/yyyy HH:mm".</summary>
    public static string FormatarDataHora(DateTime dt) => dt.ToString("dd/MM/yyyy HH:mm");
}
