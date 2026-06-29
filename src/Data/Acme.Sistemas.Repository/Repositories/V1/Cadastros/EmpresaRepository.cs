using System.Data;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;
using Acme.Sistemas.Repository.Repositories;

namespace Acme.Sistemas.Repository.Repositories.V1.Cadastros;

public sealed class EmpresaRepository : BaseRepository<Empresa>, IEmpresaRepository
{
    public EmpresaRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "empresas";
    protected override Func<IDataRecord, Empresa> Map => MapEmpresa;

    private const string Cols = @"id, tenant_id, razao_social, nome_fantasia, cnpj,
        inscricao_estadual, inscricao_municipal, email, telefone, status,
        endereco_cep, endereco_logradouro, endereco_numero, endereco_complemento,
        endereco_bairro, endereco_cidade, endereco_uf, endereco_pais,
        COALESCE(usa_rep_oficial, 0) AS usa_rep_oficial,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(Empresa e, CancellationToken cancellationToken = default)
    {
        return Db.ExecuteAsync(@"
            INSERT INTO empresas
            (id, tenant_id, razao_social, nome_fantasia, cnpj, inscricao_estadual, inscricao_municipal,
             email, telefone, status,
             endereco_cep, endereco_logradouro, endereco_numero, endereco_complemento,
             endereco_bairro, endereco_cidade, endereco_uf, endereco_pais,
             created_at, created_by)
            VALUES
            (@id, @tenant_id, @razao_social, @nome_fantasia, @cnpj, @inscricao_estadual, @inscricao_municipal,
             @email, @telefone, @status,
             @cep, @logradouro, @numero, @complemento, @bairro, @cidade, @uf, @pais,
             @created_at, @created_by)",
            BuildParameters(e, isInsert: true), cancellationToken);
    }

    public override Task UpdateAsync(Empresa e, CancellationToken cancellationToken = default)
    {
        return Db.ExecuteAsync(@"
            UPDATE empresas SET
                razao_social = @razao_social,
                nome_fantasia = @nome_fantasia,
                cnpj = @cnpj,
                inscricao_estadual = @inscricao_estadual,
                inscricao_municipal = @inscricao_municipal,
                email = @email,
                telefone = @telefone,
                status = @status,
                endereco_cep = @cep,
                endereco_logradouro = @logradouro,
                endereco_numero = @numero,
                endereco_complemento = @complemento,
                endereco_bairro = @bairro,
                endereco_cidade = @cidade,
                endereco_uf = @uf,
                endereco_pais = @pais,
                updated_at = @updated_at, updated_by = @updated_by
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            BuildParameters(e, isInsert: false), cancellationToken);
    }

    public Task<Empresa?> GetByCnpjAsync(string cnpj, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(
            $"SELECT {Cols} FROM empresas WHERE tenant_id = @tenantId AND cnpj = @cnpj AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?>
            {
                ["@tenantId"] = TenantContext.TenantId,
                ["@cnpj"] = cnpj
            }, cancellationToken);

    public Task<Empresa?> GetPrimeiraAtivaAsync(CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(
            $"SELECT {Cols} FROM empresas WHERE tenant_id = @tenantId AND status = 1 AND deleted_at IS NULL ORDER BY created_at LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId },
            cancellationToken);

    private Dictionary<string, object?> BuildParameters(Empresa e, bool isInsert)
    {
        var p = new Dictionary<string, object?>
        {
            ["@id"] = e.Id,
            ["@razao_social"] = e.RazaoSocial,
            ["@nome_fantasia"] = e.NomeFantasia,
            ["@cnpj"] = e.Cnpj,
            ["@inscricao_estadual"] = e.InscricaoEstadual,
            ["@inscricao_municipal"] = e.InscricaoMunicipal,
            ["@email"] = e.Email,
            ["@telefone"] = e.Telefone,
            ["@status"] = (int)e.Status,
            ["@cep"] = e.Endereco.Cep,
            ["@logradouro"] = e.Endereco.Logradouro,
            ["@numero"] = e.Endereco.Numero,
            ["@complemento"] = e.Endereco.Complemento,
            ["@bairro"] = e.Endereco.Bairro,
            ["@cidade"] = e.Endereco.Cidade,
            ["@uf"] = e.Endereco.Uf,
            ["@pais"] = e.Endereco.Pais
        };
        if (isInsert)
        {
            p["@tenant_id"] = TenantContext.TenantId;
            p["@created_at"] = e.CreatedAt;
            p["@created_by"] = e.CreatedBy;
        }
        else
        {
            p["@tenantId"] = TenantContext.TenantId;
            p["@updated_at"] = DateTime.UtcNow;
            p["@updated_by"] = e.UpdatedBy;
        }
        return p;
    }

    private static Empresa MapEmpresa(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        RazaoSocial = r.GetValueOrDefault<string>("razao_social") ?? string.Empty,
        NomeFantasia = r.GetValueOrDefault<string>("nome_fantasia"),
        Cnpj = r.GetValueOrDefault<string>("cnpj") ?? string.Empty,
        InscricaoEstadual = r.GetValueOrDefault<string>("inscricao_estadual"),
        InscricaoMunicipal = r.GetValueOrDefault<string>("inscricao_municipal"),
        Email = r.GetValueOrDefault<string>("email"),
        Telefone = r.GetValueOrDefault<string>("telefone"),
        Status = (StatusAtivo)r.GetValueOrDefault<int>("status"),
        Endereco = new Endereco
        {
            Cep = r.GetValueOrDefault<string>("endereco_cep"),
            Logradouro = r.GetValueOrDefault<string>("endereco_logradouro"),
            Numero = r.GetValueOrDefault<string>("endereco_numero"),
            Complemento = r.GetValueOrDefault<string>("endereco_complemento"),
            Bairro = r.GetValueOrDefault<string>("endereco_bairro"),
            Cidade = r.GetValueOrDefault<string>("endereco_cidade"),
            Uf = r.GetValueOrDefault<string>("endereco_uf"),
            Pais = r.GetValueOrDefault<string>("endereco_pais")
        },
        UsaRepOficial = r.GetValueOrDefault<int>("usa_rep_oficial") != 0,
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
