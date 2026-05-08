namespace Acme.Sistemas.Domain.Entities.Fiscal.Xml;

/// <summary>
/// Constrói a chave de acesso da NF-e (44 dígitos) e calcula o DV módulo 11.
///
/// Estrutura dos 43 dígitos base + 1 DV:
///   cUF(2) + AAMM(4) + CNPJ(14) + mod(2) + serie(3) + nNF(9) + tpEmis(1) + cNF(8)
/// </summary>
public static class ChaveAcessoBuilder
{
    /// <summary>
    /// Monta a chave de acesso completa (44 dígitos).
    /// </summary>
    /// <param name="cUF">Código IBGE da UF do emitente (2 dígitos).</param>
    /// <param name="dataEmissao">Data de emissão — só AAMM importa.</param>
    /// <param name="cnpj">CNPJ do emitente (14 dígitos, somente números).</param>
    /// <param name="modelo">Modelo do documento: "55" (NFe) ou "65" (NFCe).</param>
    /// <param name="serie">Série (1-3 dígitos, será zero-paddeada para 3).</param>
    /// <param name="numero">Número da NF-e (1-9 dígitos, será zero-paddeada para 9).</param>
    /// <param name="tpEmis">Tipo de emissão (1 dígito).</param>
    /// <param name="cNF">Código numérico aleatório (8 dígitos, zero-paddeado).</param>
    public static string Build(
        string cUF,
        DateTime dataEmissao,
        string cnpj,
        string modelo,
        string serie,
        long numero,
        int tpEmis,
        string cNF)
    {
        if (cUF.Length != 2) throw new ArgumentException("cUF deve ter 2 dígitos.", nameof(cUF));
        if (cnpj.Length != 14) throw new ArgumentException("CNPJ deve ter 14 dígitos.", nameof(cnpj));
        if (modelo.Length != 2) throw new ArgumentException("Modelo deve ter 2 dígitos.", nameof(modelo));
        if (cNF.Length != 8) throw new ArgumentException("cNF deve ter 8 dígitos.", nameof(cNF));
        if (tpEmis is < 1 or > 9) throw new ArgumentException("tpEmis deve estar entre 1 e 9.", nameof(tpEmis));

        var aamm = dataEmissao.ToString("yyMM");
        var serie3 = serie.PadLeft(3, '0');
        var nnf9 = numero.ToString().PadLeft(9, '0');
        var tpEmis1 = tpEmis.ToString();

        var base43 = $"{cUF}{aamm}{cnpj}{modelo}{serie3}{nnf9}{tpEmis1}{cNF}";
        if (base43.Length != 43)
            throw new InvalidOperationException($"Chave parcial deveria ter 43 dígitos, tem {base43.Length}: {base43}");

        var dv = CalcularDV(base43);
        return base43 + dv;
    }

    /// <summary>
    /// Calcula o dígito verificador módulo 11 da chave de 43 dígitos.
    /// Algoritmo:
    ///  1. Multiplicar cada dígito (da direita pra esquerda) por pesos 2,3,4,5,6,7,8,9 ciclando.
    ///  2. Somar os produtos.
    ///  3. Calcular resto da divisão por 11.
    ///  4. DV = 11 - resto. Se resto in {0, 1}, DV = 0.
    /// </summary>
    public static char CalcularDV(string chave43)
    {
        if (chave43.Length != 43)
            throw new ArgumentException("Chave parcial deve ter 43 dígitos.", nameof(chave43));

        var soma = 0;
        var peso = 2;
        for (var i = chave43.Length - 1; i >= 0; i--)
        {
            var c = chave43[i];
            if (!char.IsDigit(c))
                throw new ArgumentException($"Caractere inválido na chave: '{c}'.", nameof(chave43));
            soma += (c - '0') * peso;
            peso = peso == 9 ? 2 : peso + 1;
        }

        var resto = soma % 11;
        var dv = (resto == 0 || resto == 1) ? 0 : 11 - resto;
        return (char)('0' + dv);
    }

    /// <summary>
    /// Gera um cNF aleatório de 8 dígitos não-iguais ao nNF (regra Receita).
    /// </summary>
    public static string GerarCNFAleatorio(long numeroNFe, Random? random = null)
    {
        random ??= Random.Shared;
        string candidato;
        var nnf9 = numeroNFe.ToString().PadLeft(9, '0');
        do
        {
            candidato = random.Next(0, 100_000_000).ToString().PadLeft(8, '0');
        } while (candidato == nnf9[..8]); // colisão improvável, mas evitar
        return candidato;
    }
}
