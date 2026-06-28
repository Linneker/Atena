using System.Data;
using Acme.Sistemas.Domain.Entities.Referencia;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Referencia;

/// <summary>
/// Catálogo de UFs. Dado de referência nacional, não tenant-scoped — consulta direta
/// via <see cref="IDataConfiguration"/> sem filtro de tenant (à semelhança de TenantRepository).
/// </summary>
public sealed class UfRepository : IUfRepository
{
    private readonly IDataConfiguration _db;

    public UfRepository(IDataConfiguration db) => _db = db;

    public Task<IReadOnlyList<Uf>> ListAllAsync(CancellationToken cancellationToken = default)
        => _db.QueryAsync(
            "SELECT sigla, nome, codigo_ibge FROM ufs ORDER BY sigla",
            Map, null, cancellationToken);

    public Task<long> CountAsync(CancellationToken cancellationToken = default)
        => _db.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM ufs", null, cancellationToken);

    private static Uf Map(IDataRecord r) => new()
    {
        Sigla = r.GetValueOrDefault<string>("sigla") ?? string.Empty,
        Nome = r.GetValueOrDefault<string>("nome") ?? string.Empty,
        CodigoIbge = r.GetValueOrDefault<int>("codigo_ibge"),
    };
}
