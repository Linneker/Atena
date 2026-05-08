using System.Data;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;
using Acme.Sistemas.Repository.Repositories;

namespace Acme.Sistemas.Repository.Repositories.V1.Cadastros;

public sealed class FuncionarioRepository : BaseRepository<Funcionario>, IFuncionarioRepository
{
    public FuncionarioRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "funcionarios";
    protected override Func<IDataRecord, Funcionario> Map => MapEntity;

    private const string Cols = @"id, tenant_id, nome_completo, cpf, email, telefone,
        cargo, departamento, centro_de_custo_id, data_admissao, data_demissao,
        usuario_id, status,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public override Task AddAsync(Funcionario f, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO funcionarios
            (id, tenant_id, nome_completo, cpf, email, telefone,
             cargo, departamento, centro_de_custo_id, data_admissao, data_demissao,
             usuario_id, status, created_at, created_by)
            VALUES
            (@id, @tenant_id, @nome, @cpf, @email, @telefone,
             @cargo, @depto, @ccid, @adm, @dem,
             @uid, @status, @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = f.Id,
                ["@tenant_id"] = TenantContext.TenantId,
                ["@nome"] = f.NomeCompleto,
                ["@cpf"] = f.Cpf,
                ["@email"] = f.Email,
                ["@telefone"] = f.Telefone,
                ["@cargo"] = f.Cargo,
                ["@depto"] = f.Departamento,
                ["@ccid"] = f.CentroDeCustoId,
                ["@adm"] = f.DataAdmissao,
                ["@dem"] = f.DataDemissao,
                ["@uid"] = f.UsuarioId,
                ["@status"] = (int)f.Status,
                ["@created_at"] = f.CreatedAt,
                ["@created_by"] = f.CreatedBy
            }, cancellationToken);

    public override Task UpdateAsync(Funcionario f, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE funcionarios SET
                nome_completo = @nome, email = @email, telefone = @telefone,
                cargo = @cargo, departamento = @depto, centro_de_custo_id = @ccid,
                data_admissao = @adm, data_demissao = @dem,
                usuario_id = @uid, status = @status,
                updated_at = @updated_at, updated_by = @updated_by
            WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = f.Id,
                ["@tenantId"] = TenantContext.TenantId,
                ["@nome"] = f.NomeCompleto,
                ["@email"] = f.Email,
                ["@telefone"] = f.Telefone,
                ["@cargo"] = f.Cargo,
                ["@depto"] = f.Departamento,
                ["@ccid"] = f.CentroDeCustoId,
                ["@adm"] = f.DataAdmissao,
                ["@dem"] = f.DataDemissao,
                ["@uid"] = f.UsuarioId,
                ["@status"] = (int)f.Status,
                ["@updated_at"] = DateTime.UtcNow,
                ["@updated_by"] = f.UpdatedBy
            }, cancellationToken);

    public Task<Funcionario?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(
            $"SELECT {Cols} FROM funcionarios WHERE tenant_id = @tenantId AND cpf = @cpf AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@cpf"] = cpf },
            cancellationToken);

    private static Funcionario MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        NomeCompleto = r.GetValueOrDefault<string>("nome_completo") ?? string.Empty,
        Cpf = r.GetValueOrDefault<string>("cpf") ?? string.Empty,
        Email = r.GetValueOrDefault<string>("email"),
        Telefone = r.GetValueOrDefault<string>("telefone"),
        Cargo = r.GetValueOrDefault<string>("cargo"),
        Departamento = r.GetValueOrDefault<string>("departamento"),
        CentroDeCustoId = r.GetValueOrDefault<Guid?>("centro_de_custo_id"),
        DataAdmissao = r.GetValueOrDefault<DateTime?>("data_admissao"),
        DataDemissao = r.GetValueOrDefault<DateTime?>("data_demissao"),
        UsuarioId = r.GetValueOrDefault<Guid?>("usuario_id"),
        Status = (StatusAtivo)r.GetValueOrDefault<int>("status"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
