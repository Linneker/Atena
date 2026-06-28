using System.Data;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Rh;

public sealed class BeneficioCatalogoRepository : BaseRepository<BeneficioCatalogo>, IBeneficioCatalogoRepository
{
    public BeneficioCatalogoRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "beneficios_catalogo";
    protected override Func<IDataRecord, BeneficioCatalogo> Map => MapEntity;

    public override Task AddAsync(BeneficioCatalogo b, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO beneficios_catalogo
                (id, tenant_id, codigo, descricao, tipo, desconto_funcionario_pct,
                 custo_empresa_padrao, natureza_rubrica_esocial, ativo, created_at, created_by)
            VALUES (@id, @t, @codigo, @descricao, @tipo, @dfp, @cep, @nre, @ativo, @createdAt, @createdBy)",
            new Dictionary<string, object?>
            {
                ["@id"] = b.Id,
                ["@t"] = TenantContext.TenantId,
                ["@codigo"] = b.Codigo,
                ["@descricao"] = b.Descricao,
                ["@tipo"] = b.Tipo.ToString(),
                ["@dfp"] = b.DescontoFuncionarioPct,
                ["@cep"] = b.CustoEmpresaPadrao,
                ["@nre"] = b.NaturezaRubricaEsocial,
                ["@ativo"] = b.Ativo ? 1 : 0,
                ["@createdAt"] = b.CreatedAt,
                ["@createdBy"] = b.CreatedBy,
            }, cancellationToken);

    public override Task UpdateAsync(BeneficioCatalogo b, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE beneficios_catalogo SET
                codigo = @codigo, descricao = @descricao, tipo = @tipo,
                desconto_funcionario_pct = @dfp, custo_empresa_padrao = @cep,
                natureza_rubrica_esocial = @nre, ativo = @ativo,
                updated_at = @updatedAt, updated_by = @updatedBy
            WHERE id = @id AND tenant_id = @t AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = b.Id,
                ["@t"] = TenantContext.TenantId,
                ["@codigo"] = b.Codigo,
                ["@descricao"] = b.Descricao,
                ["@tipo"] = b.Tipo.ToString(),
                ["@dfp"] = b.DescontoFuncionarioPct,
                ["@cep"] = b.CustoEmpresaPadrao,
                ["@nre"] = b.NaturezaRubricaEsocial,
                ["@ativo"] = b.Ativo ? 1 : 0,
                ["@updatedAt"] = DateTime.UtcNow,
                ["@updatedBy"] = b.UpdatedBy,
            }, cancellationToken);

    public Task<BeneficioCatalogo?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(@"
            SELECT * FROM beneficios_catalogo
            WHERE tenant_id = @t AND codigo = @codigo AND deleted_at IS NULL
            LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@t"] = TenantContext.TenantId, ["@codigo"] = codigo },
            cancellationToken);

    private static BeneficioCatalogo MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Codigo = r.GetValueOrDefault<string>("codigo"),
        Descricao = r.GetValueOrDefault<string>("descricao") ?? string.Empty,
        Tipo = Enum.TryParse<TipoBeneficio>(r.GetValueOrDefault<string>("tipo"), out var t) ? t : TipoBeneficio.Outro,
        DescontoFuncionarioPct = r.GetValueOrDefault<decimal?>("desconto_funcionario_pct"),
        CustoEmpresaPadrao = r.GetValueOrDefault<decimal?>("custo_empresa_padrao"),
        NaturezaRubricaEsocial = r.GetValueOrDefault<string>("natureza_rubrica_esocial"),
        Ativo = r.GetValueOrDefault<int>("ativo") == 1,
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by"),
    };
}
