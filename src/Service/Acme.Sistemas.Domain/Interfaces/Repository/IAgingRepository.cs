namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IAgingRepository
{
    /// <summary>Aging de contas a pagar pendentes/parciais por faixa de dias até vencimento.</summary>
    Task<IReadOnlyList<(string Faixa, int Quantidade, decimal Valor)>> AgingContasPagarAsync(CancellationToken cancellationToken = default);

    /// <summary>Aging de contas a receber pendentes/parciais por faixa de dias até/desde vencimento.</summary>
    Task<IReadOnlyList<(string Faixa, int Quantidade, decimal Valor)>> AgingContasReceberAsync(CancellationToken cancellationToken = default);
}
