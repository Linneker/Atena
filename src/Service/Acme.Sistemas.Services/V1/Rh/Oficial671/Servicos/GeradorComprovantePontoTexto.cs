using System.Globalization;
using Acme.Sistemas.Domain.Interfaces.Rh;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Servicos;

/// <summary>
/// Layout texto pipe-separated do anexo II da Portaria 671/2021:
///   NSR | TIPO | CPF | PIS | DATA(yyyyMMdd) | HORA(HHmmss) | NOME | CNPJ | HASH_MARCACAO
/// Strings limpas (só dígitos para CPF/PIS/CNPJ), data/hora em UTC formatadas como
/// strings ASCII fixas. Nome truncado em 100 chars para evitar payloads colossais.
/// </summary>
public sealed class GeradorComprovantePontoTexto : IGeradorComprovantePontoTexto
{
    public string Gerar(DadosComprovante671 d)
    {
        var partes = new[]
        {
            d.Nsr.ToString("D9", CultureInfo.InvariantCulture),
            (d.TipoRegistro ?? "Entrada").Trim(),
            SoDigitos(d.CpfEmpregado),
            SoDigitos(d.PisEmpregado),
            d.DataHora.ToString("yyyyMMdd", CultureInfo.InvariantCulture),
            d.DataHora.ToString("HHmmss", CultureInfo.InvariantCulture),
            Truncar(d.NomeEmpregado ?? string.Empty, 100),
            SoDigitos(d.CnpjEmpregador),
            d.HashEncadeadoMarcacao ?? string.Empty,
        };
        return string.Join("|", partes);
    }

    private static string SoDigitos(string s) =>
        new(s?.Where(char.IsDigit).ToArray() ?? Array.Empty<char>());

    private static string Truncar(string s, int max) =>
        s.Length <= max ? s : s[..max];
}
