using Acme.Sistemas.Domain.Entities.Referencia;

namespace Acme.Sistemas.Domain.Interfaces.Repository;

/// <summary>
/// Catálogo CBO (Classificação Brasileira de Ocupações). Catálogo nacional, não tenant-scoped.
/// Tabela populada via endpoint admin opt-in (similar a CFOP).
/// </summary>
public interface ICboRepository
{
    Task<IReadOnlyList<Cbo>> ListAllAsync(CancellationToken cancellationToken = default);
    Task<Cbo?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
    Task<long> CountAsync(CancellationToken cancellationToken = default);
    Task<int> UpsertManyAsync(IEnumerable<Cbo> cbos, CancellationToken cancellationToken = default);
}
