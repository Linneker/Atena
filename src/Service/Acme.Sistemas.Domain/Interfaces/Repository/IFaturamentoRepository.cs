using Acme.Sistemas.Domain.Entities.Vendas;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IFaturamentoRepository : IBaseRepository<Faturamento>
{
    Task<IReadOnlyList<Faturamento>> ListByPedidoAsync(Guid pedidoId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Faturamento>> ListByFiltroAsync(DateTime? inicio, DateTime? fim, int skip, int take, CancellationToken cancellationToken = default);
    Task<long> CountByFiltroAsync(DateTime? inicio, DateTime? fim, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<FaturamentoItem>> ListItensAsync(Guid faturamentoId, CancellationToken cancellationToken = default);
    Task AddItensAsync(IEnumerable<FaturamentoItem> itens, CancellationToken cancellationToken = default);
    Task<int> NextNumeroAsync(CancellationToken cancellationToken = default);
    Task UpdateContaReceberAsync(Guid id, Guid contaReceberId, CancellationToken cancellationToken = default);
}
