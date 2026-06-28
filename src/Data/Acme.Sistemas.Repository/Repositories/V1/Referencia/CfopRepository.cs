using System.Data;
using Acme.Sistemas.Domain.Entities.Referencia;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Referencia;

/// <summary>Catálogo de CFOPs — referência nacional, não tenant-scoped.</summary>
public sealed class CfopRepository : ICfopRepository
{
    private readonly IDataConfiguration _db;

    public CfopRepository(IDataConfiguration db) => _db = db;

    public Task<IReadOnlyList<Cfop>> ListAsync(string? categoria, CancellationToken cancellationToken = default)
    {
        var sql = "SELECT codigo, descricao, categoria, seed_version FROM cfops";
        var parameters = new Dictionary<string, object?>();
        if (!string.IsNullOrWhiteSpace(categoria))
        {
            sql += " WHERE categoria = @categoria";
            parameters["@categoria"] = categoria;
        }
        sql += " ORDER BY codigo";
        return _db.QueryAsync(sql, Map, parameters, cancellationToken);
    }

    public Task<long> CountAsync(CancellationToken cancellationToken = default)
        => _db.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM cfops", null, cancellationToken);

    private static Cfop Map(IDataRecord r) => new()
    {
        Codigo = r.GetValueOrDefault<string>("codigo") ?? string.Empty,
        Descricao = r.GetValueOrDefault<string>("descricao") ?? string.Empty,
        Categoria = r.GetValueOrDefault<string>("categoria") ?? string.Empty,
        SeedVersion = r.GetValueOrDefault<int>("seed_version"),
    };
}
