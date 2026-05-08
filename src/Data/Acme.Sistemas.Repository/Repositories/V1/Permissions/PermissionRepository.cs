using System.Data;
using Acme.Sistemas.Domain.Entities.Permissions;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Permissions;

public sealed class PermissionRepository : IPermissionRepository
{
    private readonly IDataConfiguration _db;
    private const string Cols = "id, recurso, acao, codigo, descricao";

    public PermissionRepository(IDataConfiguration db) { _db = db; }

    public Task<Permission?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
        => _db.QueryFirstOrDefaultAsync(
            $"SELECT {Cols} FROM permissions WHERE codigo = @codigo LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@codigo"] = codigo },
            cancellationToken);

    public Task<IReadOnlyList<Permission>> ListAllAsync(CancellationToken cancellationToken = default)
        => _db.QueryAsync(
            $"SELECT {Cols} FROM permissions ORDER BY recurso, acao",
            Map, null, cancellationToken);

    public Task UpsertAsync(Permission permission, CancellationToken cancellationToken = default)
    {
        const string sql = @"INSERT INTO permissions (id, recurso, acao, codigo, descricao)
            VALUES (@id, @recurso, @acao, @codigo, @descricao)
            ON DUPLICATE KEY UPDATE recurso = @recurso, acao = @acao, descricao = @descricao";
        return _db.ExecuteAsync(sql, new Dictionary<string, object?>
        {
            ["@id"] = permission.Id,
            ["@recurso"] = permission.Recurso,
            ["@acao"] = permission.Acao,
            ["@codigo"] = permission.Codigo,
            ["@descricao"] = permission.Descricao
        }, cancellationToken);
    }

    private static Permission Map(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        Recurso = r.GetValueOrDefault<string>("recurso") ?? string.Empty,
        Acao = r.GetValueOrDefault<string>("acao") ?? string.Empty,
        Codigo = r.GetValueOrDefault<string>("codigo") ?? string.Empty,
        Descricao = r.GetValueOrDefault<string>("descricao")
    };
}
