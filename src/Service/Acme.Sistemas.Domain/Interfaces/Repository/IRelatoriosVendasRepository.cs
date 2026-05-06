namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IRelatoriosVendasRepository
{
    Task<IReadOnlyList<(Guid VendedorId, decimal Total, int Faturamentos)>> AgruparPorVendedorAsync(
        DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<(Guid ClienteId, decimal Total, int Faturamentos)>> AgruparPorClienteAsync(
        DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<(Guid ProdutoId, decimal Quantidade, decimal Total)>> AgruparPorProdutoAsync(
        DateTime inicio, DateTime fim, CancellationToken cancellationToken = default);
}
