using System.Data;
using System.Text;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;
using Acme.Sistemas.Repository.Repositories;

namespace Acme.Sistemas.Repository.Repositories.V1.Cadastros;

public sealed class FornecedorRepository : BaseRepository<Fornecedor>, IFornecedorRepository
{
    public FornecedorRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "fornecedores";
    protected override Func<IDataRecord, Fornecedor> Map => MapEntity;

    private const string Cols = @"id, tenant_id, tipo, nome, nome_fantasia, documento, inscricao_estadual,
        email, telefone, condicao_pagamento_padrao, status,
        endereco_cep, endereco_logradouro, endereco_numero, endereco_complemento,
        endereco_bairro, endereco_cidade, endereco_uf, endereco_pais,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(Fornecedor f, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO fornecedores
            (id, tenant_id, tipo, nome, nome_fantasia, documento, inscricao_estadual,
             email, telefone, condicao_pagamento_padrao, status,
             endereco_cep, endereco_logradouro, endereco_numero, endereco_complemento,
             endereco_bairro, endereco_cidade, endereco_uf, endereco_pais,
             created_at, created_by)
            VALUES
            (@id, @tenant_id, @tipo, @nome, @nf, @documento, @ie,
             @email, @telefone, @cond, @status,
             @cep, @logradouro, @numero, @complemento, @bairro, @cidade, @uf, @pais,
             @created_at, @created_by)",
            BuildParams(f, isInsert: true), cancellationToken);

    public override Task UpdateAsync(Fornecedor f, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE fornecedores SET
                tipo = @tipo, nome = @nome, nome_fantasia = @nf, documento = @documento,
                inscricao_estadual = @ie, email = @email, telefone = @telefone,
                condicao_pagamento_padrao = @cond, status = @status,
                endereco_cep = @cep, endereco_logradouro = @logradouro, endereco_numero = @numero,
                endereco_complemento = @complemento, endereco_bairro = @bairro,
                endereco_cidade = @cidade, endereco_uf = @uf, endereco_pais = @pais,
                updated_at = @updated_at, updated_by = @updated_by
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            BuildParams(f, isInsert: false), cancellationToken);

    public Task<Fornecedor?> GetByDocumentoAsync(string documento, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(
            $"SELECT {Cols} FROM fornecedores WHERE tenant_id = @tenantId AND documento = @doc AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@doc"] = documento },
            cancellationToken);

    public Task<IReadOnlyList<Fornecedor>> ListByFiltroAsync(string? termo, int skip, int take, CancellationToken cancellationToken = default)
    {
        var (sql, p) = BuildFiltro(termo);
        sql.Append(" ORDER BY nome LIMIT @take OFFSET @skip");
        p["@take"] = take;
        p["@skip"] = skip;
        return Db.QueryAsync(sql.ToString(), Map, p, cancellationToken);
    }

    public Task<long> CountByFiltroAsync(string? termo, CancellationToken cancellationToken = default)
    {
        var (sql, p) = BuildFiltro(termo, countOnly: true);
        return Db.ExecuteScalarAsync<long>(sql.ToString(), p, cancellationToken);
    }

    private (StringBuilder, Dictionary<string, object?>) BuildFiltro(string? termo, bool countOnly = false)
    {
        var sql = new StringBuilder(countOnly
            ? "SELECT COUNT(*) FROM fornecedores WHERE tenant_id = @tenantId AND deleted_at IS NULL"
            : $"SELECT {Cols} FROM fornecedores WHERE tenant_id = @tenantId AND deleted_at IS NULL");
        var p = new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId };
        if (!string.IsNullOrWhiteSpace(termo))
        {
            sql.Append(" AND (nome LIKE @termo OR documento LIKE @termo OR nome_fantasia LIKE @termo)");
            p["@termo"] = $"%{termo}%";
        }
        return (sql, p);
    }

    private Dictionary<string, object?> BuildParams(Fornecedor f, bool isInsert)
    {
        var p = new Dictionary<string, object?>
        {
            ["@id"] = f.Id,
            ["@tipo"] = (int)f.Tipo,
            ["@nome"] = f.Nome,
            ["@nf"] = f.NomeFantasia,
            ["@documento"] = f.Documento,
            ["@ie"] = f.InscricaoEstadual,
            ["@email"] = f.Email,
            ["@telefone"] = f.Telefone,
            ["@cond"] = f.CondicaoPagamentoPadrao,
            ["@status"] = (int)f.Status,
            ["@cep"] = f.Endereco.Cep,
            ["@logradouro"] = f.Endereco.Logradouro,
            ["@numero"] = f.Endereco.Numero,
            ["@complemento"] = f.Endereco.Complemento,
            ["@bairro"] = f.Endereco.Bairro,
            ["@cidade"] = f.Endereco.Cidade,
            ["@uf"] = f.Endereco.Uf,
            ["@pais"] = f.Endereco.Pais
        };
        if (isInsert)
        {
            p["@tenant_id"] = TenantContext.TenantId;
            p["@created_at"] = f.CreatedAt;
            p["@created_by"] = f.CreatedBy;
        }
        else
        {
            p["@tenantId"] = TenantContext.TenantId;
            p["@updated_at"] = DateTime.UtcNow;
            p["@updated_by"] = f.UpdatedBy;
        }
        return p;
    }

    private static Fornecedor MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        Tipo = (TipoPessoa)r.GetValueOrDefault<int>("tipo"),
        Nome = r.GetValueOrDefault<string>("nome") ?? string.Empty,
        NomeFantasia = r.GetValueOrDefault<string>("nome_fantasia"),
        Documento = r.GetValueOrDefault<string>("documento") ?? string.Empty,
        InscricaoEstadual = r.GetValueOrDefault<string>("inscricao_estadual"),
        Email = r.GetValueOrDefault<string>("email"),
        Telefone = r.GetValueOrDefault<string>("telefone"),
        CondicaoPagamentoPadrao = r.GetValueOrDefault<string>("condicao_pagamento_padrao"),
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
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
