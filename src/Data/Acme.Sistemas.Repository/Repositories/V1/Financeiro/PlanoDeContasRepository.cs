using System.Data;
using Acme.Sistemas.Domain.Entities.Financeiro;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Financeiro;

public sealed class PlanoDeContasRepository : BaseRepository<PlanoDeContas>, IPlanoDeContasRepository
{
    public PlanoDeContasRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "plano_de_contas";
    protected override Func<IDataRecord, PlanoDeContas> Map => MapEntity;

    private const string Cols = @"id, tenant_id, codigo, nome, tipo, pai_id, nivel,
        aceita_lancamento, ativo,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(PlanoDeContas p, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO plano_de_contas
            (id, tenant_id, codigo, nome, tipo, pai_id, nivel, aceita_lancamento, ativo,
             created_at, created_by)
            VALUES
            (@id, @tenant_id, @codigo, @nome, @tipo, @pai_id, @nivel, @aceita, @ativo,
             @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = p.Id,
                ["@tenant_id"] = TenantContext.TenantId,
                ["@codigo"] = p.Codigo,
                ["@nome"] = p.Nome,
                ["@tipo"] = (int)p.Tipo,
                ["@pai_id"] = p.PaiId,
                ["@nivel"] = p.Nivel,
                ["@aceita"] = p.Aceita_Lancamento ? 1 : 0,
                ["@ativo"] = p.Ativo ? 1 : 0,
                ["@created_at"] = p.CreatedAt,
                ["@created_by"] = p.CreatedBy
            }, cancellationToken);

    public override Task UpdateAsync(PlanoDeContas p, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE plano_de_contas SET
                codigo = @codigo, nome = @nome, tipo = @tipo,
                pai_id = @pai_id, nivel = @nivel,
                aceita_lancamento = @aceita, ativo = @ativo,
                updated_at = @updated_at, updated_by = @updated_by
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = p.Id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@codigo"] = p.Codigo,
                ["@nome"] = p.Nome,
                ["@tipo"] = (int)p.Tipo,
                ["@pai_id"] = p.PaiId,
                ["@nivel"] = p.Nivel,
                ["@aceita"] = p.Aceita_Lancamento ? 1 : 0,
                ["@ativo"] = p.Ativo ? 1 : 0,
                ["@updated_at"] = DateTime.UtcNow,
                ["@updated_by"] = p.UpdatedBy
            }, cancellationToken);

    public Task<PlanoDeContas?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(
            $"SELECT {Cols} FROM plano_de_contas WHERE tenant_id = @tenantId AND codigo = @codigo AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?>
            {
                ["@tenantId"] = TenantContext.TenantId,
                ["@codigo"] = codigo
            }, cancellationToken);

    public Task<IReadOnlyList<PlanoDeContas>> ListAllAsync(CancellationToken cancellationToken = default)
        => Db.QueryAsync(
            $"SELECT {Cols} FROM plano_de_contas WHERE tenant_id = @tenantId AND deleted_at IS NULL ORDER BY codigo",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId },
            cancellationToken);

    public Task<IReadOnlyList<PlanoDeContas>> ListFilhosAsync(Guid paiId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(
            $"SELECT {Cols} FROM plano_de_contas WHERE tenant_id = @tenantId AND pai_id = @paiId AND deleted_at IS NULL ORDER BY codigo",
            Map,
            new Dictionary<string, object?>
            {
                ["@tenantId"] = TenantContext.TenantId,
                ["@paiId"] = paiId
            }, cancellationToken);

    public async Task<bool> HasFilhosAsync(Guid paiId, CancellationToken cancellationToken = default)
    {
        var count = await Db.ExecuteScalarAsync<long>(
            "SELECT COUNT(*) FROM plano_de_contas WHERE tenant_id = @tenantId AND pai_id = @paiId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@tenantId"] = TenantContext.TenantId,
                ["@paiId"] = paiId
            }, cancellationToken);
        return count > 0;
    }

    private static PlanoDeContas MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Codigo = r.GetValueOrDefault<string>("codigo") ?? string.Empty,
        Nome = r.GetValueOrDefault<string>("nome") ?? string.Empty,
        Tipo = (TipoConta)r.GetValueOrDefault<int>("tipo"),
        PaiId = r.GetValueOrDefault<Guid?>("pai_id"),
        Nivel = r.GetValueOrDefault<int>("nivel"),
        Aceita_Lancamento = r.GetValueOrDefault<int>("aceita_lancamento") == 1,
        Ativo = r.GetValueOrDefault<int>("ativo") == 1,
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
