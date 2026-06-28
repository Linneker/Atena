using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Engine;

/// <summary>
/// Calcula movimentos do banco de horas a partir de uma lista de ResumoDia
/// e uma política. Função pura. Não persiste — quem persiste é o handler.
/// </summary>
public static class CalculadoraSaldoBancoHoras
{
    public sealed record MovimentoPlanejado(
        DateOnly Data,
        int Minutos,
        OrigemMovimentoBancoHoras Origem,
        Guid? ReferenciaMarcacaoId,
        string? Observacao);

    public sealed record SaldoCalculado(
        string Competencia,
        IReadOnlyList<MovimentoPlanejado> Movimentos,
        int SaldoTotalMinutos,
        int HorasDevidasMinutos,
        int HorasRealizadasMinutos);

    public static SaldoCalculado Calcular(
        string competencia,
        IReadOnlyList<CalculadoraJornadaDiaria.ResumoDia> resumos,
        BancoHorasPolitica? politica)
    {
        var movs = new List<MovimentoPlanejado>();
        var devido = 0;
        var realizado = 0;
        var saldoAcumulado = 0;

        var limiteMinutos = (int)((politica?.LimiteHorasAcumular ?? 40m) * 60m);

        foreach (var r in resumos)
        {
            devido += r.EsperadoMinutos;
            realizado += r.TrabalhadoMinutos;

            if (r.EhFeriado && r.TrabalhadoMinutos > 0)
            {
                // Trabalho em feriado entra como HE bruta no banco
                movs.Add(new MovimentoPlanejado(r.Data, r.TrabalhadoMinutos,
                    OrigemMovimentoBancoHoras.Acumulo, null,
                    $"Trabalho em feriado ({r.DiaSemana})"));
                saldoAcumulado += r.TrabalhadoMinutos;
            }
            else if (r.EhDiaUtil && r.SaldoMinutos != 0)
            {
                var origem = r.SaldoMinutos > 0
                    ? OrigemMovimentoBancoHoras.Acumulo
                    : OrigemMovimentoBancoHoras.Compensacao;
                movs.Add(new MovimentoPlanejado(r.Data, r.SaldoMinutos, origem, null, null));
                saldoAcumulado += r.SaldoMinutos;
            }
        }

        // Expira excedente acima do limite (política conservadora).
        if (saldoAcumulado > limiteMinutos)
        {
            var excedente = saldoAcumulado - limiteMinutos;
            movs.Add(new MovimentoPlanejado(
                resumos.LastOrDefault()?.Data ?? DateOnly.FromDateTime(DateTime.UtcNow),
                -excedente,
                OrigemMovimentoBancoHoras.Expiracao,
                null,
                $"Excedente acima do limite ({limiteMinutos / 60}h) zerado conforme política."));
            saldoAcumulado = limiteMinutos;
        }

        return new SaldoCalculado(competencia, movs, saldoAcumulado, devido, realizado);
    }
}
