using Acme.Sistemas.Domain.Entities;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TEntity>> ListAsync(int skip = 0, int take = 50, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<long> CountAsync(CancellationToken cancellationToken = default);
}
