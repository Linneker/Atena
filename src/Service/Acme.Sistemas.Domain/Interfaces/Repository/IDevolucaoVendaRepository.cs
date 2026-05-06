using Acme.Sistemas.Domain.Entities.Vendas;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IDevolucaoVendaRepository : IBaseRepository<DevolucaoVenda>
{
    Task<IReadOnlyList<DevolucaoVenda>> ListByFaturamentoAsync(Guid faturamentoId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DevolucaoVendaItem>> ListItensAsync(Guid devolucaoId, CancellationToken cancellationToken = default);
    Task AddItensAsync(IEnumerable<DevolucaoVendaItem> itens, CancellationToken cancellationToken = default);
}
