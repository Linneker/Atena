namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IRelatoriosFinanceirosRepository
{
    /// <summary>Soma valor_pago de contas_pagar agrupado por plano_de_contas_id no período.</summary>
    Task<IReadOnlyDictionary<Guid, decimal>> AggregateContasPagarPorPlanoAsync(
        DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);

    /// <summary>Soma valor_recebido de contas_receber agrupado por plano_de_contas_id no período.</summary>
    Task<IReadOnlyDictionary<Guid, decimal>> AggregateContasReceberPorPlanoAsync(
        DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);

    /// <summary>Saldo total de contas a receber pendentes/parciais (ativo gerencial).</summary>
    Task<decimal> TotalContasReceberPendentesAsync(CancellationToken cancellationToken = default);

    /// <summary>Saldo total de contas a pagar pendentes/parciais (passivo gerencial).</summary>
    Task<decimal> TotalContasPagarPendentesAsync(CancellationToken cancellationToken = default);

    /// <summary>Saldo total de dívidas em aberto (passivo gerencial).</summary>
    Task<decimal> TotalDividasAbertasAsync(CancellationToken cancellationToken = default);
}
