using System.Data;
using Acme.Sistemas.Domain.Entities.Referencia;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Referencia;

/// <summary>Catálogo de CSTs — referência nacional, não tenant-scoped. 4 tabelas por imposto.</summary>
public sealed class CstRepository : ICstRepository
{
    private readonly IDataConfiguration _db;

    public CstRepository(IDataConfiguration db) => _db = db;

    private static string? TableFor(string tipo) => tipo?.ToLowerInvariant() switch
    {
        "icms" => "csts_icms",
        "pis" => "csts_pis",
        "cofins" => "csts_cofins",
        "ipi" => "csts_ipi",
        _ => null,
    };

    public Task<IReadOnlyList<Cst>> ListByTipoAsync(string tipo, CancellationToken cancellationToken = default)
    {
        var table = TableFor(tipo);
        if (table is null)
            return Task.FromResult<IReadOnlyList<Cst>>(Array.Empty<Cst>());

        var normalized = tipo.ToLowerInvariant();
        return _db.QueryAsync(
            $"SELECT codigo, descricao FROM {table} ORDER BY codigo",
            r => new Cst
            {
                Tipo = normalized,
                Codigo = r.GetValueOrDefault<string>("codigo") ?? string.Empty,
                Descricao = r.GetValueOrDefault<string>("descricao") ?? string.Empty,
            },
            null, cancellationToken);
    }

    public Task<long> CountAsync(string tipo, CancellationToken cancellationToken = default)
    {
        var table = TableFor(tipo);
        return table is null
            ? Task.FromResult(0L)
            : _db.ExecuteScalarAsync<long>($"SELECT COUNT(*) FROM {table}", null, cancellationToken);
    }
}
