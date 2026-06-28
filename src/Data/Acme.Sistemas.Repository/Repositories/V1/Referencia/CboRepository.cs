using System.Data;
using Acme.Sistemas.Domain.Entities.Referencia;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Referencia;

public sealed class CboRepository : ICboRepository
{
    private readonly IDataConfiguration _db;

    public CboRepository(IDataConfiguration db) => _db = db;

    public Task<IReadOnlyList<Cbo>> ListAllAsync(CancellationToken cancellationToken = default)
        => _db.QueryAsync(
            "SELECT codigo, titulo, grande_grupo, familia, ativo FROM cbos WHERE ativo = 1 ORDER BY codigo",
            Map, null, cancellationToken);

    public Task<Cbo?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
        => _db.QueryFirstOrDefaultAsync(
            "SELECT codigo, titulo, grande_grupo, familia, ativo FROM cbos WHERE codigo = @c LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@c"] = codigo },
            cancellationToken);

    public Task<long> CountAsync(CancellationToken cancellationToken = default)
        => _db.ExecuteScalarAsync<long>("SELECT COUNT(*) FROM cbos", null, cancellationToken);

    public async Task<int> UpsertManyAsync(IEnumerable<Cbo> cbos, CancellationToken cancellationToken = default)
    {
        var count = 0;
        foreach (var c in cbos)
        {
            await _db.ExecuteAsync(@"
                INSERT INTO cbos (codigo, titulo, grande_grupo, familia, ativo)
                VALUES (@c, @t, @gg, @f, @a)
                ON DUPLICATE KEY UPDATE
                    titulo = VALUES(titulo),
                    grande_grupo = VALUES(grande_grupo),
                    familia = VALUES(familia),
                    ativo = VALUES(ativo)",
                new Dictionary<string, object?>
                {
                    ["@c"] = c.Codigo,
                    ["@t"] = c.Titulo,
                    ["@gg"] = c.GrandeGrupo,
                    ["@f"] = c.Familia,
                    ["@a"] = c.Ativo ? 1 : 0,
                }, cancellationToken);
            count++;
        }
        return count;
    }

    private static Cbo Map(IDataRecord r) => new()
    {
        Codigo = r.GetValueOrDefault<string>("codigo") ?? string.Empty,
        Titulo = r.GetValueOrDefault<string>("titulo") ?? string.Empty,
        GrandeGrupo = r.GetValueOrDefault<string>("grande_grupo"),
        Familia = r.GetValueOrDefault<string>("familia"),
        Ativo = r.GetValueOrDefault<int>("ativo") == 1,
    };
}
