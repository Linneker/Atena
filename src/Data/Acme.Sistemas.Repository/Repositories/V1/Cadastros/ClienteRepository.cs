using System.Data;
using System.Text;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;
using Acme.Sistemas.Repository.Helper;
using Acme.Sistemas.Repository.Repositories;

namespace Acme.Sistemas.Repository.Repositories.V1.Cadastros;

public sealed class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
{
    public ClienteRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "clientes";
    protected override Func<IDataRecord, Cliente> Map => MapEntity;

    private const string Cols = @"id, tenant_id, tipo, nome, nome_fantasia, documento, inscricao_estadual,
        email, telefone, status, inadimplente, bloqueado_vendas,
        endereco_cep, endereco_logradouro, endereco_numero, endereco_complemento,
        endereco_bairro, endereco_cidade, endereco_uf, endereco_pais,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(Cliente c, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO clientes
            (id, tenant_id, tipo, nome, nome_fantasia, documento, inscricao_estadual,
             email, telefone, status, inadimplente, bloqueado_vendas,
             endereco_cep, endereco_logradouro, endereco_numero, endereco_complemento,
             endereco_bairro, endereco_cidade, endereco_uf, endereco_pais,
             created_at, created_by)
            VALUES
            (@id, @tenant_id, @tipo, @nome, @nf, @documento, @ie,
             @email, @telefone, @status, @inad, @bloq,
             @cep, @logradouro, @numero, @complemento, @bairro, @cidade, @uf, @pais,
             @created_at, @created_by)",
            BuildParams(c, isInsert: true), cancellationToken);

    public override Task UpdateAsync(Cliente c, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE clientes SET
                tipo = @tipo, nome = @nome, nome_fantasia = @nf, documento = @documento,
                inscricao_estadual = @ie, email = @email, telefone = @telefone, status = @status,
                endereco_cep = @cep, endereco_logradouro = @logradouro, endereco_numero = @numero,
                endereco_complemento = @complemento, endereco_bairro = @bairro,
                endereco_cidade = @cidade, endereco_uf = @uf, endereco_pais = @pais,
                updated_at = @updated_at, updated_by = @updated_by
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            BuildParams(c, isInsert: false), cancellationToken);

    public Task UpdateInadimplenciaAsync(Guid id, bool inadimplente, bool bloqueadoVendas, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE clientes SET
                inadimplente = @inad, bloqueado_vendas = @bloq, updated_at = @updated_at
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@inad"] = inadimplente ? 1 : 0,
                ["@bloq"] = bloqueadoVendas ? 1 : 0,
                ["@updated_at"] = DateTime.UtcNow
            }, cancellationToken);

    public Task<Cliente?> GetByDocumentoAsync(string documento, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(
            $"SELECT {Cols} FROM clientes WHERE tenant_id = @tenantId AND documento = @doc AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@doc"] = documento },
            cancellationToken);

    public Task<IReadOnlyList<Cliente>> ListByFiltroAsync(
        string? termo, bool? inadimplente, int skip, int take,
        CancellationToken cancellationToken = default)
    {
        var (sql, p) = BuildFiltro(termo, inadimplente);
        sql.Append(" ORDER BY nome LIMIT @take OFFSET @skip");
        p["@take"] = take;
        p["@skip"] = skip;
        return Db.QueryAsync(sql.ToString(), Map, p, cancellationToken);
    }

    public Task<long> CountByFiltroAsync(string? termo, bool? inadimplente, CancellationToken cancellationToken = default)
    {
        var (sql, p) = BuildFiltro(termo, inadimplente, countOnly: true);
        return Db.ExecuteScalarAsync<long>(sql.ToString(), p, cancellationToken);
    }

    private (StringBuilder, Dictionary<string, object?>) BuildFiltro(string? termo, bool? inadimplente, bool countOnly = false)
    {
        var sql = new StringBuilder(countOnly
            ? "SELECT COUNT(*) FROM clientes WHERE tenant_id = @tenantId AND deleted_at IS NULL"
            : $"SELECT {Cols} FROM clientes WHERE tenant_id = @tenantId AND deleted_at IS NULL");
        var p = new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId };
        if (!string.IsNullOrWhiteSpace(termo))
        {
            sql.Append(" AND (nome LIKE @termo OR documento LIKE @termo OR nome_fantasia LIKE @termo)");
            p["@termo"] = $"%{termo}%";
        }
        if (inadimplente.HasValue)
        {
            sql.Append(" AND inadimplente = @inad");
            p["@inad"] = inadimplente.Value ? 1 : 0;
        }
        return (sql, p);
    }

    private Dictionary<string, object?> BuildParams(Cliente c, bool isInsert)
    {
        var p = new Dictionary<string, object?>
        {
            ["@id"] = c.Id,
            ["@tipo"] = (int)c.Tipo,
            ["@nome"] = c.Nome,
            ["@nf"] = c.NomeFantasia,
            ["@documento"] = c.Documento,
            ["@ie"] = c.InscricaoEstadual,
            ["@email"] = c.Email,
            ["@telefone"] = c.Telefone,
            ["@status"] = (int)c.Status,
            ["@cep"] = c.Endereco.Cep,
            ["@logradouro"] = c.Endereco.Logradouro,
            ["@numero"] = c.Endereco.Numero,
            ["@complemento"] = c.Endereco.Complemento,
            ["@bairro"] = c.Endereco.Bairro,
            ["@cidade"] = c.Endereco.Cidade,
            ["@uf"] = c.Endereco.Uf,
            ["@pais"] = c.Endereco.Pais
        };
        if (isInsert)
        {
            p["@tenant_id"] = TenantContext.TenantId;
            p["@inad"] = c.Inadimplente ? 1 : 0;
            p["@bloq"] = c.BloqueadoVendas ? 1 : 0;
            p["@created_at"] = c.CreatedAt;
            p["@created_by"] = c.CreatedBy;
        }
        else
        {
            p["@tenantId"] = TenantContext.TenantId;
            p["@updated_at"] = DateTime.UtcNow;
            p["@updated_by"] = c.UpdatedBy;
        }
        return p;
    }

    private static Cliente MapEntity(IDataRecord r) => new()
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
        Status = (StatusAtivo)r.GetValueOrDefault<int>("status"),
        Inadimplente = r.GetValueOrDefault<int>("inadimplente") == 1,
        BloqueadoVendas = r.GetValueOrDefault<int>("bloqueado_vendas") == 1,
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
