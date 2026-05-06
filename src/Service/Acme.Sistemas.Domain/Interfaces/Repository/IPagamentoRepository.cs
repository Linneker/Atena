using Acme.Sistemas.Domain.Entities.Financeiro;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IPagamentoRepository : IBaseRepository<Pagamento>
{
    Task<IReadOnlyList<Pagamento>> ListByContaPagarAsync(Guid contaPagarId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Pagamento>> ListByDividaAsync(Guid dividaId, CancellationToken cancellationToken = default);
}
