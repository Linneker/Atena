using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using FuncionarioEntity = Acme.Sistemas.Domain.Entities.Cadastros.Funcionario;
using JornadaEntity = Acme.Sistemas.Domain.Entities.Rh.Jornada;
using MarcacaoPontoEntity = Acme.Sistemas.Domain.Entities.Rh.MarcacaoPonto;
using BancoHorasPoliticaEntity = Acme.Sistemas.Domain.Entities.Rh.BancoHorasPolitica;
using FeriadoEntity = Acme.Sistemas.Domain.Entities.Rh.Feriado;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Engine;

/// <summary>
/// Gera estrutura JSON do espelho mensal a partir das marcações + jornada + política.
/// </summary>
public static class GeradorEspelhoMensal
{
    public sealed record EspelhoDia(
        DateOnly Data,
        string DiaSemana,
        bool EhFeriado,
        bool EhDiaUtil,
        string? JanelaEsperadaEntrada,
        string? JanelaEsperadaSaida,
        IReadOnlyList<EspelhoBatida> Batidas,
        int TrabalhadoMinutos,
        int EsperadoMinutos,
        int SaldoMinutos,
        int AtrasoMinutos,
        IReadOnlyList<string> Anomalias);

    public sealed record EspelhoBatida(Guid Id, string Hora, string Tipo, string Origem);

    public sealed record EspelhoTotais(
        int DiasUteis,
        int DiasTrabalhados,
        int DiasFalta,
        int TrabalhadoMinutos,
        int EsperadoMinutos,
        int SaldoMesMinutos,
        int HorasExtrasMinutos,
        int SaldoBancoAcumuladoMinutos);

    public sealed record EspelhoMensal(
        Guid FuncionarioId,
        string FuncionarioNome,
        string FuncionarioCpf,
        string Competencia,
        EspelhoJornada JornadaVigente,
        EspelhoPolitica? PoliticaBancoHoras,
        IReadOnlyList<EspelhoDia> Dias,
        EspelhoTotais Totais,
        string HashEspelho,
        DateTime GeradoEm);

    public sealed record EspelhoJornada(string Nome, decimal CargaSemanal);
    public sealed record EspelhoPolitica(string Nome, int LimiteAcumularMinutos);

    public static EspelhoMensal Gerar(
        FuncionarioEntity funcionario,
        string competencia,
        JornadaEntity jornada,
        BancoHorasPoliticaEntity? politica,
        IReadOnlyList<MarcacaoPontoEntity> marcacoes,
        IReadOnlyList<FeriadoEntity> feriados)
    {
        if (!DateOnly.TryParseExact(competencia + "-01", "yyyy-MM-dd", out var primeiroDia))
            throw new ArgumentException($"competência '{competencia}' inválida; esperado YYYY-MM.");

        var ultimoDia = primeiroDia.AddMonths(1).AddDays(-1);
        var feriadosSet = feriados.Select(f => f.Data).ToHashSet();

        var dias = new List<EspelhoDia>();
        var resumos = new List<CalculadoraJornadaDiaria.ResumoDia>();
        var totaisDiasFalta = 0;
        var totaisDiasTrab = 0;
        var totaisDiasUteis = 0;
        var totaisHorasExtras = 0;

        for (var d = primeiroDia; d <= ultimoDia; d = d.AddDays(1))
        {
            var ehFeriado = feriadosSet.Contains(d);
            var batidasDia = marcacoes
                .Where(m => DateOnly.FromDateTime(m.DataHora) == d)
                .OrderBy(m => m.DataHora)
                .Select(m => new PareadorBatidas.BatidaInput(m.Id, m.DataHora, m.Tipo, m.Origem))
                .ToList();

            var resumo = CalculadoraJornadaDiaria.Calcular(d, jornada, batidasDia, ehFeriado);
            resumos.Add(resumo);

            if (resumo.EhDiaUtil) totaisDiasUteis++;
            if (resumo.TrabalhadoMinutos > 0) totaisDiasTrab++;
            else if (resumo.EhDiaUtil) totaisDiasFalta++;
            totaisHorasExtras += resumo.HorasExtrasBrutasMinutos;

            var janelaInfo = LerJanela(jornada.JanelasJson, resumo.DiaSemana);

            dias.Add(new EspelhoDia(
                d, resumo.DiaSemana, ehFeriado, resumo.EhDiaUtil,
                janelaInfo?.entrada, janelaInfo?.saida,
                batidasDia.Select(b => new EspelhoBatida(
                    b.Id, b.DataHora.ToString("HH:mm"), b.Tipo?.ToString() ?? "?", b.Origem.ToString())).ToList(),
                resumo.TrabalhadoMinutos, resumo.EsperadoMinutos, resumo.SaldoMinutos,
                resumo.AtrasoMinutos, resumo.Anomalias));
        }

        var saldoBanco = CalculadoraSaldoBancoHoras.Calcular(competencia, resumos, politica);
        var totalTrab = resumos.Sum(r => r.TrabalhadoMinutos);
        var totalEsp = resumos.Sum(r => r.EsperadoMinutos);

        var totais = new EspelhoTotais(
            totaisDiasUteis, totaisDiasTrab, totaisDiasFalta,
            totalTrab, totalEsp, totalTrab - totalEsp,
            totaisHorasExtras, saldoBanco.SaldoTotalMinutos);

        var espelho = new EspelhoMensal(
            funcionario.Id, funcionario.NomeCompleto, funcionario.Cpf, competencia,
            new EspelhoJornada(jornada.Nome, jornada.CargaSemanalHoras),
            politica is null ? null : new EspelhoPolitica(politica.Nome, (int)(politica.LimiteHorasAcumular * 60m)),
            dias, totais,
            HashEspelho: ComputarHashEspelho(funcionario.Id, competencia, dias, totais),
            GeradoEm: DateTime.UtcNow);

        return espelho;
    }

    private static string ComputarHashEspelho(
        Guid funcionarioId, string competencia,
        IReadOnlyList<EspelhoDia> dias, EspelhoTotais totais)
    {
        var sb = new StringBuilder();
        sb.Append(funcionarioId).Append('|').Append(competencia).Append('|');
        foreach (var d in dias)
            sb.Append(d.Data).Append(':').Append(d.TrabalhadoMinutos).Append('|');
        sb.Append("T:").Append(totais.TrabalhadoMinutos).Append(':').Append(totais.SaldoMesMinutos);

        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(sb.ToString()));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private static (string entrada, string saida)? LerJanela(string janelasJson, string dia)
    {
        try
        {
            using var doc = JsonDocument.Parse(janelasJson);
            if (doc.RootElement.ValueKind != JsonValueKind.Array) return null;
            foreach (var el in doc.RootElement.EnumerateArray())
            {
                if (el.TryGetProperty("dia", out var dEl) &&
                    string.Equals(dEl.GetString(), dia, StringComparison.OrdinalIgnoreCase))
                {
                    var ent = el.TryGetProperty("entrada", out var e) ? e.GetString() : null;
                    var sai = el.TryGetProperty("saida", out var s) ? s.GetString() : null;
                    if (ent is not null && sai is not null) return (ent, sai);
                }
            }
        }
        catch (JsonException) { }
        return null;
    }
}
