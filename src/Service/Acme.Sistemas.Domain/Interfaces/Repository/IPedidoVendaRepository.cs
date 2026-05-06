using Acme.Sistemas.Domain.Entities.Vendas;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IPedidoVendaRepository : IBaseRepository<PedidoVenda>
{
    Task<IReadOnlyList<PedidoVenda>> ListByFiltroAsync(StatusPedidoVenda? status, Guid? clienteId, Guid? vendedorId, DateTime? inicio, DateTime? fim, int skip, int take, CancellationToken cancellationToken = default);
    Task<long> CountByFiltroAsync(StatusPedidoVenda? status, Guid? clienteId, Guid? vendedorId, DateTime? inicio, DateTime? fim, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PedidoVendaItem>> ListItensAsync(Guid pedidoId, CancellationToken cancellationToken = default);
    Task AddItensAsync(IEnumerable<PedidoVendaItem> itens, CancellationToken cancellationToken = default);
    Task<int> NextNumeroAsync(CancellationToken cancellationToken = default);
    Task UpdateStatusAsync(Guid id, StatusPedidoVenda status, CancellationToken cancellationToken = default);
    Task UpdateItemQuantidadeFaturadaAsync(Guid itemId, decimal novaQuantidadeFaturada, CancellationToken cancellationToken = default);
}
