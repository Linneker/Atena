using System.Data;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Rh;

public sealed class LotacaoRepository : BaseRepository<Lotacao>, ILotacaoRepository
{
    public LotacaoRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "lotacoes";
    protected override Func<IDataRecord, Lotacao> Map => MapEntity;

    public override Task AddAsync(Lotacao l, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO lotacoes
                (id, tenant_id, nome, empresa_id, cnpj, endereco_json, ativo, created_at, created_by)
            VALUES (@id, @t, @nome, @emp, @cnpj, @endereco, @ativo, @createdAt, @createdBy)",
            new Dictionary<string, object?>
            {
                ["@id"] = l.Id,
                ["@t"] = TenantContext.TenantId,
                ["@nome"] = l.Nome,
                ["@emp"] = l.EmpresaId,
                ["@cnpj"] = l.Cnpj,
                ["@endereco"] = l.EnderecoJson,
                ["@ativo"] = l.Ativo ? 1 : 0,
                ["@createdAt"] = l.CreatedAt,
                ["@createdBy"] = l.CreatedBy,
            }, cancellationToken);

    public override Task UpdateAsync(Lotacao l, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE lotacoes SET
                nome = @nome, empresa_id = @emp, cnpj = @cnpj, endereco_json = @endereco, ativo = @ativo,
                updated_at = @updatedAt, updated_by = @updatedBy
            WHERE id = @id AND tenant_id = @t AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = l.Id,
                ["@t"] = TenantContext.TenantId,
                ["@nome"] = l.Nome,
                ["@emp"] = l.EmpresaId,
                ["@cnpj"] = l.Cnpj,
                ["@endereco"] = l.EnderecoJson,
                ["@ativo"] = l.Ativo ? 1 : 0,
                ["@updatedAt"] = DateTime.UtcNow,
                ["@updatedBy"] = l.UpdatedBy,
            }, cancellationToken);

    public Task<Lotacao?> GetByNomeAsync(string nome, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(@"
            SELECT * FROM lotacoes
            WHERE tenant_id = @t AND nome = @nome AND deleted_at IS NULL
            LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@t"] = TenantContext.TenantId, ["@nome"] = nome },
            cancellationToken);

    private static Lotacao MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Nome = r.GetValueOrDefault<string>("nome") ?? string.Empty,
        EmpresaId = r.GetValueOrDefault<Guid?>("empresa_id"),
        Cnpj = r.GetValueOrDefault<string>("cnpj"),
        EnderecoJson = r.GetValueOrDefault<string>("endereco_json"),
        Ativo = r.GetValueOrDefault<int>("ativo") == 1,
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by"),
    };
}
