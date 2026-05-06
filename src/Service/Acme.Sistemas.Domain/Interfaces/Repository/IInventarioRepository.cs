using Acme.Sistemas.Domain.Entities.Estoque;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IInventarioRepository : IBaseRepository<Inventario>
{
    Task<IReadOnlyList<InventarioItem>> ListItensAsync(Guid inventarioId, CancellationToken cancellationToken = default);
    Task AddItensAsync(IEnumerable<InventarioItem> itens, CancellationToken cancellationToken = default);
    Task UpdateItemAsync(InventarioItem item, CancellationToken cancellationToken = default);
    Task FecharAsync(Guid inventarioId, DateTime dataFechamento, CancellationToken cancellationToken = default);
}
