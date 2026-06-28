using System.Data;
using Acme.Sistemas.Domain.Entities.Referencia;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Referencia;

/// <summary>Catálogo de códigos de serviço LC 116/03 — referência nacional, não tenant-scoped.</summary>
public sealed class CodigoServicoLc116Repository : ICodigoServicoLc116Repository
{
    private readonly IDataConfiguration _db;

    public CodigoServicoLc116Repository(IDataConfiguration db) => _db = db;

    public Task<IReadOnlyList<CodigoServicoLc116>> ListAllAsync(CancellationToken cancellationToken = default)
        => _db.QueryAsync(
            "SELECT codigo, descricao FROM codigos_servico_lc116 ORDER BY codigo",
            Map, null, cancellationToken);

    public Task<long> CountAsync(CancellationToken cancellationToken = default)
        => _db.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM codigos_servico_lc116", null, cancellationToken);

    private static CodigoServicoLc116 Map(IDataRecord r) => new()
    {
        Codigo = r.GetValueOrDefault<string>("codigo") ?? string.Empty,
        Descricao = r.GetValueOrDefault<string>("descricao") ?? string.Empty,
    };
}
