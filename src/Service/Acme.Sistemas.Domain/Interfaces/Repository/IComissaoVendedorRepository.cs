using Acme.Sistemas.Domain.Entities.Vendas;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IComissaoVendedorRepository : IBaseRepository<ComissaoVendedor>
{
    Task<IReadOnlyList<ComissaoVendedor>> ListByVendedorAsync(Guid vendedorId, DateTime? inicio, DateTime? fim, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ComissaoVendedor>> ListByFaturamentoAsync(Guid faturamentoId, CancellationToken cancellationToken = default);
}
