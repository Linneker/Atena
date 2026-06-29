using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Entities.Rh.Oficial671;
using EmpresaEntity = Acme.Sistemas.Domain.Entities.Cadastros.Empresa;
using FuncionarioEntity = Acme.Sistemas.Domain.Entities.Cadastros.Funcionario;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Afd;

/// <summary>
/// Writer do layout AFD versão 003 da Portaria MTP 671/2021 anexo I.
/// Layout texto fixo, 7 tipos de registro:
///   tipo 1 — cabeçalho (CNPJ, CEI, razão, endereço, período, geração)
///   tipo 2 — identificador do REP (versão, INPI)
///   tipo 3 — marcações (NSR, data, hora, PIS)
///   tipo 4 — ajustes de RTC (relógio do REP)
///   tipo 5 — empregados (PIS, CPF, nome)
///   tipo 6 — eventos REP (inicialização, manutenção)
///   tipo 9 — trailer com totalizadores e HASH SHA-256 do conteúdo
/// MVP: tipos 1, 2, 3, 5, 9 cobrem os cenários esperados; 4 e 6 emitidos vazios
/// (aguardam telemetria do REP real — TODO PR `rh-671-rtc-eventos`).
/// </summary>
public sealed class LayoutAfd003Writer
{
    public AfdResult Escrever(AfdContexto ctx)
    {
        var sb = new StringBuilder();
        var nsrCabecalho = 1L;

        // Tipo 1 — Cabeçalho
        sb.AppendLine(string.Concat(
            Pad(nsrCabecalho.ToString("D9"), 9),
            "1",
            Pad(SoDigitos(ctx.Empresa.Cnpj), 14),
            Pad(ctx.Config.Cno ?? string.Empty, 14),  // CEI/CNO
            Pad(ctx.Empresa.RazaoSocial, 30),
            Pad(MontarEndereco(ctx.Empresa), 100),
            ctx.PeriodoInicio.ToString("yyyyMMdd", CultureInfo.InvariantCulture),
            ctx.PeriodoFim.ToString("yyyyMMdd", CultureInfo.InvariantCulture),
            ctx.GeradoEm.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture),
            Pad("003", 3)));

        // Tipo 2 — Identificador do REP
        sb.AppendLine(string.Concat(
            Pad(2L.ToString("D9"), 9),
            "2",
            Pad(SoDigitos(ctx.Empresa.Cnpj), 14),
            Pad("ATENA-REP-C", 17),
            Pad(ctx.GeradoEm.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture), 14)));

        var nsr = 3L;

        // Tipo 5 — empregados (1 registro por empregado distinto no período)
        foreach (var func in ctx.Funcionarios.DistinctBy(f => f.Id))
        {
            sb.AppendLine(string.Concat(
                Pad(nsr++.ToString("D9"), 9),
                "5",
                Pad(SoDigitos(func.Pis ?? string.Empty), 12),
                Pad(SoDigitos(func.Cpf ?? string.Empty), 11),
                Pad(func.NomeCompleto ?? string.Empty, 52)));
        }

        // Tipo 3 — marcações ordenadas por NSR original do comprovante
        foreach (var (m, c) in ctx.Marcacoes
                     .Join(ctx.Comprovantes, m => m.Id, c => c.MarcacaoId, (m, c) => (m, c))
                     .OrderBy(t => t.c.Nsr))
        {
            var func = ctx.Funcionarios.FirstOrDefault(f => f.Id == m.FuncionarioId);
            sb.AppendLine(string.Concat(
                Pad(c.Nsr.ToString("D9"), 9),
                "3",
                m.DataHora.ToString("yyyyMMdd", CultureInfo.InvariantCulture),
                m.DataHora.ToString("HHmm", CultureInfo.InvariantCulture),
                Pad(SoDigitos(func?.Pis ?? string.Empty), 12)));
        }

        // Tipo 9 — Trailer: contagem de tipo 2..6 + hash SHA-256 do conteúdo
        var totalMarcacoes = ctx.Marcacoes.Count;
        var totalEmpregados = ctx.Funcionarios.DistinctBy(f => f.Id).Count();
        var conteudoBytes = Encoding.UTF8.GetBytes(sb.ToString());
        var hash = Convert.ToHexString(SHA256.HashData(conteudoBytes)).ToLowerInvariant();

        sb.AppendLine(string.Concat(
            Pad(9L.ToString("D9"), 9),
            "9",
            Pad(totalMarcacoes.ToString("D9"), 9),
            Pad(totalEmpregados.ToString("D9"), 9),
            Pad("0", 9),                          // total ajustes RTC (não emitido nesta versão MVP)
            Pad("0", 9),                          // total eventos REP
            hash));

        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        return new AfdResult(bytes, hash);
    }

    private static string Pad(string s, int len)
    {
        s ??= string.Empty;
        return s.Length >= len ? s[..len] : s.PadRight(len);
    }
    private static string SoDigitos(string s) =>
        new(s?.Where(char.IsDigit).ToArray() ?? Array.Empty<char>());
    private static string MontarEndereco(EmpresaEntity e)
    {
        var partes = new[]
        {
            e.Endereco.Logradouro, e.Endereco.Numero,
            e.Endereco.Bairro, e.Endereco.Cidade, e.Endereco.Uf,
        };
        return string.Join(" ", partes.Where(p => !string.IsNullOrWhiteSpace(p)));
    }
}

public sealed record AfdContexto(
    EmpresaEntity Empresa,
    ConfiguracaoRep Config,
    DateTime PeriodoInicio,
    DateTime PeriodoFim,
    DateTime GeradoEm,
    IReadOnlyList<MarcacaoPonto> Marcacoes,
    IReadOnlyList<ComprovantePonto> Comprovantes,
    IReadOnlyList<FuncionarioEntity> Funcionarios);

public sealed record AfdResult(byte[] Conteudo, string HashSha256Hex);
