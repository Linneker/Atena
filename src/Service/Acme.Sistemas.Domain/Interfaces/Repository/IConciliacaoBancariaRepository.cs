using Acme.Sistemas.Domain.Entities.Financeiro;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IConciliacaoBancariaRepository : IBaseRepository<ConciliacaoBancaria>
{
    Task<IReadOnlyList<ItemExtrato>> ListItensAsync(Guid conciliacaoId, CancellationToken cancellationToken = default);
    Task AddItensAsync(IEnumerable<ItemExtrato> itens, CancellationToken cancellationToken = default);
    Task UpdateItemAsync(ItemExtrato item, CancellationToken cancellationToken = default);
    Task UpdateTotaisAsync(Guid conciliacaoId, int totalLancamentos, int totalConciliados, StatusConciliacao status, CancellationToken cancellationToken = default);
}
