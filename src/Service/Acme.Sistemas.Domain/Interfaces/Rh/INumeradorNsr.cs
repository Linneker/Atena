namespace Acme.Sistemas.Domain.Interfaces.Rh;

/// <summary>
/// Reserva NSR (Número Sequencial de Registro) único e monotonicamente crescente por
/// (tenant, empresa) — Portaria MTP 671/2021. Atômico via INSERT … ON DUPLICATE KEY UPDATE
/// (mesma mecânica do <c>NumeradorNFe</c>). Pulos são proibidos pela Portaria — uma reserva
/// é sempre consumida; auditoria de gaps fica no <c>JobAuditarGapsNsrWorker</c>.
/// </summary>
public interface INumeradorNsr
{
    /// <summary>Reserva o próximo NSR — concorrência segura.</summary>
    Task<long> ProximoAsync(Guid empresaId, CancellationToken cancellationToken = default);

    /// <summary>Último NSR conhecido (sem incrementar). Usado em auditoria e cabeçalho AFD.</summary>
    Task<long> UltimoAsync(Guid empresaId, CancellationToken cancellationToken = default);
}
