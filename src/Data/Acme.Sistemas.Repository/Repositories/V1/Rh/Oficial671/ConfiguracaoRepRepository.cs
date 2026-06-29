using System.Data;
using Acme.Sistemas.Domain.Entities.Rh.Oficial671;
using Acme.Sistemas.Domain.Enums.Rh;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Rh.Oficial671;

public sealed class ConfiguracaoRepRepository : BaseRepository<ConfiguracaoRep>, IConfiguracaoRepRepository
{
    public ConfiguracaoRepRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "configuracao_rep";
    protected override Func<IDataRecord, ConfiguracaoRep> Map => MapEntity;

    public override Task AddAsync(ConfiguracaoRep c, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO configuracao_rep
                (id, tenant_id, empresa_id, tipo, razao_social, cnpj_cei, cno,
                 inscricao_estadual, cnae_principal, endereco_logradouro, endereco_numero,
                 endereco_complemento, endereco_bairro, endereco_cidade, endereco_uf, endereco_cep,
                 certificado_id, responsavel_cpf, responsavel_nome, created_at, created_by)
            VALUES (@id, @t, @e, @tipo, @rs, @cnpj, @cno, @ie, @cnae, @log, @num, @comp,
                    @bairro, @cidade, @uf, @cep, @cert, @cpf, @nome, @createdAt, @createdBy)",
            BuildParams(c), cancellationToken);

    public override Task UpdateAsync(ConfiguracaoRep c, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE configuracao_rep SET
                tipo = @tipo, razao_social = @rs, cnpj_cei = @cnpj, cno = @cno,
                inscricao_estadual = @ie, cnae_principal = @cnae,
                endereco_logradouro = @log, endereco_numero = @num, endereco_complemento = @comp,
                endereco_bairro = @bairro, endereco_cidade = @cidade, endereco_uf = @uf, endereco_cep = @cep,
                certificado_id = @cert, responsavel_cpf = @cpf, responsavel_nome = @nome,
                updated_at = @updatedAt, updated_by = @updatedBy
            WHERE id = @id AND tenant_id = @t AND deleted_at IS NULL",
            BuildParams(c, updating: true), cancellationToken);

    public Task<ConfiguracaoRep?> GetByEmpresaAsync(Guid empresaId, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(@"
            SELECT * FROM configuracao_rep
            WHERE tenant_id = @t AND empresa_id = @e AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?>
            {
                ["@t"] = TenantContext.TenantId,
                ["@e"] = empresaId,
            }, cancellationToken);

    private Dictionary<string, object?> BuildParams(ConfiguracaoRep c, bool updating = false) => new()
    {
        ["@id"] = c.Id, ["@t"] = TenantContext.TenantId,
        ["@e"] = c.EmpresaId, ["@tipo"] = c.Tipo.ToString(),
        ["@rs"] = c.RazaoSocial, ["@cnpj"] = c.CnpjCei, ["@cno"] = c.Cno,
        ["@ie"] = c.InscricaoEstadual, ["@cnae"] = c.CnaePrincipal,
        ["@log"] = c.EnderecoLogradouro, ["@num"] = c.EnderecoNumero,
        ["@comp"] = c.EnderecoComplemento, ["@bairro"] = c.EnderecoBairro,
        ["@cidade"] = c.EnderecoCidade, ["@uf"] = c.EnderecoUf, ["@cep"] = c.EnderecoCep,
        ["@cert"] = c.CertificadoId,
        ["@cpf"] = c.ResponsavelCpf, ["@nome"] = c.ResponsavelNome,
        ["@createdAt"] = c.CreatedAt, ["@createdBy"] = c.CreatedBy,
        ["@updatedAt"] = updating ? DateTime.UtcNow : (object?)null,
        ["@updatedBy"] = c.UpdatedBy,
    };

    private static ConfiguracaoRep MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        EmpresaId = r.GetValueOrDefault<Guid>("empresa_id"),
        Tipo = Enum.TryParse<TipoRep>(r.GetValueOrDefault<string>("tipo"), out var t) ? t : TipoRep.RepC,
        RazaoSocial = r.GetValueOrDefault<string>("razao_social") ?? string.Empty,
        CnpjCei = r.GetValueOrDefault<string>("cnpj_cei") ?? string.Empty,
        Cno = r.GetValueOrDefault<string>("cno"),
        InscricaoEstadual = r.GetValueOrDefault<string>("inscricao_estadual"),
        CnaePrincipal = r.GetValueOrDefault<string>("cnae_principal"),
        EnderecoLogradouro = r.GetValueOrDefault<string>("endereco_logradouro") ?? string.Empty,
        EnderecoNumero = r.GetValueOrDefault<string>("endereco_numero"),
        EnderecoComplemento = r.GetValueOrDefault<string>("endereco_complemento"),
        EnderecoBairro = r.GetValueOrDefault<string>("endereco_bairro"),
        EnderecoCidade = r.GetValueOrDefault<string>("endereco_cidade") ?? string.Empty,
        EnderecoUf = r.GetValueOrDefault<string>("endereco_uf") ?? string.Empty,
        EnderecoCep = r.GetValueOrDefault<string>("endereco_cep"),
        CertificadoId = r.GetValueOrDefault<Guid>("certificado_id"),
        ResponsavelCpf = r.GetValueOrDefault<string>("responsavel_cpf") ?? string.Empty,
        ResponsavelNome = r.GetValueOrDefault<string>("responsavel_nome") ?? string.Empty,
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by"),
    };
}
