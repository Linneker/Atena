namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IDashboardRepository
{
    Task<int> CountVendasAbertasAsync(CancellationToken cancellationToken = default);
    Task<int> CountContasReceberVencendoAsync(int diasJanela, CancellationToken cancellationToken = default);
    Task<int> CountContasPagarVencendoAsync(int diasJanela, CancellationToken cancellationToken = default);
    Task<int> CountProdutosEmEstoqueCriticoAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<(int Ano, int Mes, decimal Receitas, decimal Despesas)>> EvolucaoFinanceiraUltimosMesesAsync(
        int meses, CancellationToken cancellationToken = default);
}
