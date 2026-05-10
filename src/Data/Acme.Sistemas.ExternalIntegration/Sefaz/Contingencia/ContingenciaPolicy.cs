using System.Collections.Concurrent;
using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.Domain.Entities.Fiscal.Xml.Servicos;
using Acme.Sistemas.ExternalIntegration.Sefaz.Servicos;

namespace Acme.Sistemas.ExternalIntegration.Sefaz.Contingencia;

public enum EstadoContingencia
{
    Operando,
    Indisponivel,
}

/// <summary>
/// Estado de contingência por (UF origem, ambiente). In-memory, por nó.
/// Para deploy multi-réplica, este estado fica isolado por processo; é aceitável porque:
/// - Cada nó re-testa o serviço a cada minuto via worker.
/// - A pior consequência é uma NFe a mais ou a menos indo via SVRS por nó.
/// </summary>
public sealed record ContingenciaInfo(
    EstadoContingencia Estado,
    DateTime DesdeUtc,
    DateTime? RetomarTesteEmUtc,
    string? UltimoCStat,
    string? UltimoMotivo);

/// <summary>
/// Política de contingência SVRS:
/// 1) Tenta SEFAZ-Origem (UF do emitente).
/// 2) Em timeout/cStat=108/109/erro de rede, marca origem como indisponível por
///    `JanelaIndisponibilidade` (5 min default).
/// 3) `UrlParaUsar(uf, amb)` decide UF efetiva: "UF" ou "SVRS".
/// 4) Worker externo chama <see cref="RegistrarRespostaStatusServico"/> a cada minuto.
/// </summary>
public sealed class ContingenciaPolicy
{
    private readonly TimeSpan _janelaIndisponibilidade;
    private readonly ConcurrentDictionary<string, ContingenciaInfo> _estado = new(StringComparer.Ordinal);

    public ContingenciaPolicy(TimeSpan? janelaIndisponibilidade = null)
    {
        _janelaIndisponibilidade = janelaIndisponibilidade ?? TimeSpan.FromMinutes(5);
    }

    /// <summary>
    /// Decide a UF que deve ser usada para a próxima transmissão.
    /// </summary>
    /// <returns>
    /// - "SVRS" se a origem está marcada indisponível e a janela ainda não expirou.
    /// - <paramref name="uf"/> caso contrário (ou se a janela expirou — fica em modo "tentar de novo").
    /// </returns>
    public string UfParaUsar(string uf, AmbienteFiscal ambiente)
    {
        var key = Key(uf, ambiente);
        if (_estado.TryGetValue(key, out var info)
            && info.Estado == EstadoContingencia.Indisponivel
            && info.RetomarTesteEmUtc is { } retomar
            && retomar > DateTime.UtcNow)
        {
            return "SVRS";
        }
        return uf;
    }

    /// <summary>
    /// Estado atual (não-muta — apenas leitura).
    /// </summary>
    public ContingenciaInfo? GetEstado(string uf, AmbienteFiscal ambiente)
        => _estado.TryGetValue(Key(uf, ambiente), out var info) ? info : null;

    /// <summary>
    /// Hook chamado após uma transmissão "real": se houve timeout ou paralisação,
    /// marca a origem como indisponível.
    /// </summary>
    public void RegistrarRespostaTransmissao(string uf, AmbienteFiscal ambiente, string? cStat, string? motivo, bool erroDeRede)
    {
        if (erroDeRede || SefazResultadoCodigo.IsParalisacao(cStat))
        {
            MarcarIndisponivel(uf, ambiente, cStat, motivo ?? (erroDeRede ? "Erro de rede / timeout" : "SEFAZ paralisada"));
        }
    }

    /// <summary>
    /// Hook chamado pelo worker periódico após <c>NFeStatusServico4</c>: se cStat=107,
    /// volta para Operando; se 108/109, mantém/inicia indisponibilidade.
    /// </summary>
    public void RegistrarRespostaStatusServico(string uf, AmbienteFiscal ambiente, StatusServicoResultado resultado)
    {
        if (resultado.Operando)
        {
            // Saiu da contingência — limpa estado
            _estado.TryRemove(Key(uf, ambiente), out _);
            return;
        }

        if (resultado.Paralisado)
        {
            MarcarIndisponivel(uf, ambiente, resultado.CStat, resultado.XMotivo);
        }
    }

    /// <summary>
    /// Força entrada manual em contingência (operação humana — útil quando o operador detecta
    /// degradação antes do worker).
    /// </summary>
    public void ForcarContingencia(string uf, AmbienteFiscal ambiente, string motivo)
        => MarcarIndisponivel(uf, ambiente, cStat: null, motivo: motivo);

    /// <summary>
    /// Força saída manual de contingência (operação humana).
    /// </summary>
    public void LimparContingencia(string uf, AmbienteFiscal ambiente)
        => _estado.TryRemove(Key(uf, ambiente), out _);

    private void MarcarIndisponivel(string uf, AmbienteFiscal ambiente, string? cStat, string? motivo)
    {
        var agora = DateTime.UtcNow;
        _estado[Key(uf, ambiente)] = new ContingenciaInfo(
            Estado: EstadoContingencia.Indisponivel,
            DesdeUtc: agora,
            RetomarTesteEmUtc: agora.Add(_janelaIndisponibilidade),
            UltimoCStat: cStat,
            UltimoMotivo: motivo);
    }

    private static string Key(string uf, AmbienteFiscal ambiente) => $"{uf}|{ambiente}";
}
