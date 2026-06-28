using System.Text.Json;
using JornadaEntity = Acme.Sistemas.Domain.Entities.Rh.Jornada;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Engine;

/// <summary>
/// Calcula o resumo do dia: minutos trabalhados, esperados, saldo, HE bruta (sem
/// adicional — engine de folha aplica em W6), atrasos, faltas. Função pura.
/// </summary>
public static class CalculadoraJornadaDiaria
{
    public sealed record ResumoDia(
        DateOnly Data,
        string DiaSemana,
        bool EhFeriado,
        bool EhDiaUtil,
        int EsperadoMinutos,
        int TrabalhadoMinutos,
        int SaldoMinutos,
        int HorasExtrasBrutasMinutos,
        int AtrasoMinutos,
        IReadOnlyList<PareadorBatidas.IntervaloTrabalhado> Intervalos,
        IReadOnlyList<string> Anomalias);

    /// <summary>
    /// Calcula o resumo do dia para um funcionário com jornada conhecida.
    /// Lê janelasJson da jornada para descobrir entrada/saida esperadas do dia da semana.
    /// </summary>
    public static ResumoDia Calcular(
        DateOnly data,
        JornadaEntity jornada,
        IReadOnlyList<PareadorBatidas.BatidaInput> batidasDoDia,
        bool ehFeriado)
    {
        var diaSemana = data.DayOfWeek;
        var diaSemanaPt = DiaSemanaPtBr(diaSemana);

        var janela = LerJanelaDoDia(jornada.JanelasJson, diaSemanaPt);
        var esperadoMinutos = janela?.MinutosEsperados ?? 0;
        var ehDiaUtil = !ehFeriado && esperadoMinutos > 0;

        var pareamento = PareadorBatidas.Parear(batidasDoDia);
        var trabalhado = pareamento.TotalTrabalhadoMinutos;

        var saldo = ehDiaUtil ? trabalhado - esperadoMinutos : trabalhado;
        var heBruta = ehDiaUtil && trabalhado > esperadoMinutos ? trabalhado - esperadoMinutos
                    : (!ehDiaUtil ? trabalhado : 0);
        var atraso = 0;
        if (ehDiaUtil && janela is { } j && batidasDoDia.Count > 0)
        {
            var primeiraEntrada = batidasDoDia.OrderBy(b => b.DataHora).First().DataHora;
            var entradaEsperada = data.ToDateTime(j.Entrada);
            var tolerancia = jornada.ToleranciaMinutos;
            var atrasoMin = (int)Math.Round((primeiraEntrada - entradaEsperada).TotalMinutes);
            if (atrasoMin > tolerancia) atraso = atrasoMin - tolerancia;
        }

        var anomalias = new List<string>(pareamento.Anomalias);
        if (ehDiaUtil && batidasDoDia.Count == 0)
            anomalias.Add("Falta — nenhuma batida registrada em dia útil.");
        if (ehDiaUtil && trabalhado > 0 && trabalhado < esperadoMinutos / 2)
            anomalias.Add($"Trabalhado ({trabalhado}min) muito abaixo do esperado ({esperadoMinutos}min).");

        return new ResumoDia(
            data, diaSemanaPt, ehFeriado, ehDiaUtil,
            esperadoMinutos, trabalhado, saldo, heBruta, atraso,
            pareamento.Intervalos, anomalias);
    }

    public sealed record JanelaDia(TimeOnly Entrada, TimeOnly Saida, int MinutosEsperados);

    private static JanelaDia? LerJanelaDoDia(string janelasJson, string diaSemanaPt)
    {
        try
        {
            using var doc = JsonDocument.Parse(janelasJson);
            if (doc.RootElement.ValueKind != JsonValueKind.Array) return null;

            foreach (var el in doc.RootElement.EnumerateArray())
            {
                var dia = el.TryGetProperty("dia", out var d) ? d.GetString() : null;
                if (!string.Equals(dia, diaSemanaPt, StringComparison.OrdinalIgnoreCase)) continue;

                var entrada = el.TryGetProperty("entrada", out var e) ? e.GetString() : null;
                var saida = el.TryGetProperty("saida", out var s) ? s.GetString() : null;
                if (entrada is null || saida is null) continue;

                if (!TimeOnly.TryParse(entrada, out var te) || !TimeOnly.TryParse(saida, out var ts))
                    continue;

                var minutos = (int)Math.Round((ts - te).TotalMinutes);
                return new JanelaDia(te, ts, minutos);
            }
        }
        catch (JsonException) { }
        return null;
    }

    private static string DiaSemanaPtBr(DayOfWeek d) => d switch
    {
        DayOfWeek.Sunday => "dom",
        DayOfWeek.Monday => "seg",
        DayOfWeek.Tuesday => "ter",
        DayOfWeek.Wednesday => "qua",
        DayOfWeek.Thursday => "qui",
        DayOfWeek.Friday => "sex",
        DayOfWeek.Saturday => "sab",
        _ => "?",
    };
}
