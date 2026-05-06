using Acme.Sistemas.Domain.Entities.Estoque;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IEstoqueRepository : IBaseRepository<Estoque>
{
    Task<Estoque?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
}
