namespace Acme.Sistemas.Domain.Interfaces.Fiscal;

/// <summary>
/// Reserva números sequenciais únicos por (tenant, CNPJ emitente, série) com lock pessimista.
/// Pulos são proibidos por lei fiscal — uma reserva é sempre consumida.
/// Se o emitente quiser descartar uma faixa, deve usar NFeInutilizacao4 (cliente SEFAZ).
/// </summary>
public interface INumeradorNFe
{
    /// <summary>
    /// Reserva o próximo número da sequência. Atômico — duas chamadas concorrentes
    /// retornam números distintos consecutivos.
    /// </summary>
    Task<long> ProximoAsync(string cnpj, int serie, CancellationToken cancellationToken = default);

    /// <summary>
    /// Ajusta o último número da sequência. Usado após inutilização (define ultimo_numero = nNFFin).
    /// </summary>
    Task AjustarUltimoNumeroAsync(string cnpj, int serie, long ultimoNumero, CancellationToken cancellationToken = default);
}
