using Acme.Sistemas.Domain.Entities.Vendas;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IOrcamentoRepository : IBaseRepository<Orcamento>
{
    Task<IReadOnlyList<Orcamento>> ListByFiltroAsync(StatusOrcamento? status, Guid? clienteId, int skip, int take, CancellationToken cancellationToken = default);
    Task<long> CountByFiltroAsync(StatusOrcamento? status, Guid? clienteId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<OrcamentoItem>> ListItensAsync(Guid orcamentoId, CancellationToken cancellationToken = default);
    Task AddItensAsync(IEnumerable<OrcamentoItem> itens, CancellationToken cancellationToken = default);
    Task<int> NextNumeroAsync(CancellationToken cancellationToken = default);
    Task UpdateStatusAsync(Guid id, StatusOrcamento status, CancellationToken cancellationToken = default);
}
