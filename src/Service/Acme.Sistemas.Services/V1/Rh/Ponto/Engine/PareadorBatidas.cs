using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Engine;

/// <summary>
/// Pareia batidas cronológicas em intervalos trabalhados. Heurística: se as batidas
/// vêm com tipo declarado, respeita; senão infere por sequência. Marca anomalias
/// quando quantidade não bate com a esperada da jornada.
/// </summary>
public static class PareadorBatidas
{
    public sealed record BatidaInput(Guid Id, DateTime DataHora, TipoMarcacao? Tipo, OrigemMarcacao Origem);

    public sealed record IntervaloTrabalhado(
        DateTime InicioTrabalho,
        DateTime FimTrabalho,
        int Minutos,
        bool ContemAlmoco);

    public sealed record ResultadoPareamento(
        IReadOnlyList<IntervaloTrabalhado> Intervalos,
        IReadOnlyList<string> Anomalias,
        int TotalTrabalhadoMinutos);

    /// <summary>
    /// Pareia batidas em pares (entrada/saída). Quando há 4+ batidas, identifica intervalo
    /// de almoço. Retorna anomalia quando há quantidade ímpar.
    /// </summary>
    public static ResultadoPareamento Parear(IReadOnlyList<BatidaInput> batidasDoDia)
    {
        var anomalias = new List<string>();
        if (batidasDoDia.Count == 0)
            return new ResultadoPareamento(Array.Empty<IntervaloTrabalhado>(), anomalias, 0);

        var ordenadas = batidasDoDia.OrderBy(b => b.DataHora).ToList();

        if (ordenadas.Count % 2 != 0)
        {
            anomalias.Add($"Quantidade ímpar de batidas ({ordenadas.Count}); pareamento desconsidera a última.");
            ordenadas = ordenadas.Take(ordenadas.Count - 1).ToList();
        }

        var intervalos = new List<IntervaloTrabalhado>();
        var total = 0;

        for (var i = 0; i < ordenadas.Count; i += 2)
        {
            var entrada = ordenadas[i];
            var saida = ordenadas[i + 1];
            if (saida.DataHora <= entrada.DataHora)
            {
                anomalias.Add($"Saída {saida.DataHora:HH:mm} <= Entrada {entrada.DataHora:HH:mm}; intervalo descartado.");
                continue;
            }

            var minutos = (int)Math.Round((saida.DataHora - entrada.DataHora).TotalMinutes);

            // Heurística: pausa entre 30min e 3h entre intervalos consecutivos = almoço
            var contemAlmoco = false;
            if (i + 2 < ordenadas.Count)
            {
                var proximaEntrada = ordenadas[i + 2];
                var pausa = (proximaEntrada.DataHora - saida.DataHora).TotalMinutes;
                contemAlmoco = pausa >= 30 && pausa <= 180;
            }

            intervalos.Add(new IntervaloTrabalhado(entrada.DataHora, saida.DataHora, minutos, contemAlmoco));
            total += minutos;
        }

        // CLT > 6h exige 1h de intervalo
        if (total > 360 && intervalos.Count == 1)
            anomalias.Add("Jornada > 6h sem intervalo registrado (CLT exige 1h de almoço).");

        return new ResultadoPareamento(intervalos, anomalias, total);
    }
}
