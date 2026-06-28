using System.Data;
using Acme.Sistemas.Domain.Entities.Financeiro;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Financeiro;

public sealed class CentroDeCustoRepository : BaseRepository<CentroDeCusto>, ICentroDeCustoRepository
{
    public CentroDeCustoRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "centros_de_custo";
    protected override Func<IDataRecord, CentroDeCusto> Map => MapEntity;

    private const string Cols = @"id, tenant_id, codigo, nome, descricao, responsavel_id, ativo,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(CentroDeCusto c, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO centros_de_custo
            (id, tenant_id, codigo, nome, descricao, responsavel_id, ativo,
             created_at, created_by)
            VALUES
            (@id, @tenant_id, @codigo, @nome, @descricao, @resp, @ativo, @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = c.Id,
                ["@tenant_id"] = TenantContext.TenantId,
                ["@codigo"] = c.Codigo,
                ["@nome"] = c.Nome,
                ["@descricao"] = c.Descricao,
                ["@resp"] = c.ResponsavelId,
                ["@ativo"] = c.Ativo ? 1 : 0,
                ["@created_at"] = c.CreatedAt,
                ["@created_by"] = c.CreatedBy
            }, cancellationToken);

    public override Task UpdateAsync(CentroDeCusto c, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE centros_de_custo SET
                codigo = @codigo, nome = @nome, descricao = @descricao,
                responsavel_id = @resp, ativo = @ativo,
                updated_at = @updated_at, updated_by = @updated_by
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = c.Id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@codigo"] = c.Codigo,
                ["@nome"] = c.Nome,
                ["@descricao"] = c.Descricao,
                ["@resp"] = c.ResponsavelId,
                ["@ativo"] = c.Ativo ? 1 : 0,
                ["@updated_at"] = DateTime.UtcNow,
                ["@updated_by"] = c.UpdatedBy
            }, cancellationToken);

    public Task<CentroDeCusto?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(
            $"SELECT {Cols} FROM centros_de_custo WHERE tenant_id = @tenantId AND codigo = @codigo AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?>
            {
                ["@tenantId"] = TenantContext.TenantId,
                ["@codigo"] = codigo
            }, cancellationToken);

    public async Task<IReadOnlyDictionary<Guid, string>> GetNomesByIdsAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        var list = ids?.Distinct().ToList() ?? new List<Guid>();
        if (list.Count == 0) return new Dictionary<Guid, string>();

        var parameters = new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId };
        var placeholders = new List<string>(list.Count);
        for (var i = 0; i < list.Count; i++)
        {
            var name = $"@id{i}";
            placeholders.Add(name);
            parameters[name] = list[i];
        }

        var sql = $@"SELECT id, nome FROM centros_de_custo
                     WHERE tenant_id = @tenantId AND deleted_at IS NULL
                       AND id IN ({string.Join(", ", placeholders)})";

        var rows = await Db.QueryAsync(
            sql,
            r => (Id: r.GetValueOrDefault<Guid>("id"), Nome: r.GetValueOrDefault<string>("nome") ?? string.Empty),
            parameters,
            cancellationToken);

        return rows.ToDictionary(x => x.Id, x => x.Nome);
    }

    public Task<long> CountVinculosAsync(Guid centroId, CancellationToken cancellationToken = default)
        => Db.ExecuteScalarAsync<long>(@"
            SELECT
              (SELECT COUNT(*) FROM despesas WHERE tenant_id = @tenantId AND centro_de_custo_id = @id AND deleted_at IS NULL) +
              (SELECT COUNT(*) FROM receitas WHERE tenant_id = @tenantId AND centro_de_custo_id = @id AND deleted_at IS NULL)",
            new Dictionary<string, object?>
            {
                ["@tenantId"] = TenantContext.TenantId,
                ["@id"] = centroId
            }, cancellationToken);

    private static CentroDeCusto MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Codigo = r.GetValueOrDefault<string>("codigo") ?? string.Empty,
        Nome = r.GetValueOrDefault<string>("nome") ?? string.Empty,
        Descricao = r.GetValueOrDefault<string>("descricao"),
        ResponsavelId = r.GetValueOrDefault<Guid?>("responsavel_id"),
        Ativo = r.GetValueOrDefault<int>("ativo") == 1,
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
