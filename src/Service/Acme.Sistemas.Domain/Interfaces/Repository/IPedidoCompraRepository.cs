using Acme.Sistemas.Domain.Entities.Compras;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IPedidoCompraRepository : IBaseRepository<PedidoCompra>
{
    Task<PedidoCompra?> GetByNumeroAsync(string numero, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PedidoCompra>> ListByFiltroAsync(
        StatusPedidoCompra? status, Guid? fornecedorId, int skip, int take, CancellationToken cancellationToken = default);
    Task<long> CountByFiltroAsync(StatusPedidoCompra? status, Guid? fornecedorId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PedidoCompraItem>> ListItensAsync(Guid pedidoId, CancellationToken cancellationToken = default);
    Task AddItensAsync(IEnumerable<PedidoCompraItem> itens, CancellationToken cancellationToken = default);

    Task<int> NextNumeroAsync(CancellationToken cancellationToken = default);
    Task UpdateStatusAsync(Guid id, StatusPedidoCompra status, CancellationToken cancellationToken = default);
}
