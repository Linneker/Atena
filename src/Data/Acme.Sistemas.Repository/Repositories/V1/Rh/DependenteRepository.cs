using System.Data;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Rh;

public sealed class DependenteRepository : BaseRepository<Dependente>, IDependenteRepository
{
    public DependenteRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "dependentes";
    protected override Func<IDataRecord, Dependente> Map => MapEntity;

    public override Task AddAsync(Dependente d, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO dependentes
                (id, tenant_id, funcionario_id, nome_completo, cpf, data_nascimento, tipo,
                 irrf, salario_familia, pensao_alimenticia_pct, data_inicio, data_fim,
                 created_at, created_by)
            VALUES (@id, @t, @fid, @nome, @cpf, @nasc, @tipo,
                    @irrf, @sf, @pap, @ini, @fim, @createdAt, @createdBy)",
            new Dictionary<string, object?>
            {
                ["@id"] = d.Id,
                ["@t"] = TenantContext.TenantId,
                ["@fid"] = d.FuncionarioId,
                ["@nome"] = d.NomeCompleto,
                ["@cpf"] = d.Cpf,
                ["@nasc"] = d.DataNascimento.ToDateTime(TimeOnly.MinValue),
                ["@tipo"] = d.Tipo.ToString(),
                ["@irrf"] = d.Irrf ? 1 : 0,
                ["@sf"] = d.SalarioFamilia ? 1 : 0,
                ["@pap"] = d.PensaoAlimenticiaPct,
                ["@ini"] = d.DataInicio?.ToDateTime(TimeOnly.MinValue),
                ["@fim"] = d.DataFim?.ToDateTime(TimeOnly.MinValue),
                ["@createdAt"] = d.CreatedAt,
                ["@createdBy"] = d.CreatedBy,
            }, cancellationToken);

    public override Task UpdateAsync(Dependente d, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE dependentes SET
                nome_completo = @nome, cpf = @cpf, data_nascimento = @nasc, tipo = @tipo,
                irrf = @irrf, salario_familia = @sf, pensao_alimenticia_pct = @pap,
                data_inicio = @ini, data_fim = @fim,
                updated_at = @updatedAt, updated_by = @updatedBy
            WHERE id = @id AND tenant_id = @t AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = d.Id,
                ["@t"] = TenantContext.TenantId,
                ["@nome"] = d.NomeCompleto,
                ["@cpf"] = d.Cpf,
                ["@nasc"] = d.DataNascimento.ToDateTime(TimeOnly.MinValue),
                ["@tipo"] = d.Tipo.ToString(),
                ["@irrf"] = d.Irrf ? 1 : 0,
                ["@sf"] = d.SalarioFamilia ? 1 : 0,
                ["@pap"] = d.PensaoAlimenticiaPct,
                ["@ini"] = d.DataInicio?.ToDateTime(TimeOnly.MinValue),
                ["@fim"] = d.DataFim?.ToDateTime(TimeOnly.MinValue),
                ["@updatedAt"] = DateTime.UtcNow,
                ["@updatedBy"] = d.UpdatedBy,
            }, cancellationToken);

    public Task<IReadOnlyList<Dependente>> ListByFuncionarioAsync(
        Guid funcionarioId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(@"
            SELECT * FROM dependentes
            WHERE tenant_id = @t AND funcionario_id = @fid AND deleted_at IS NULL
            ORDER BY nome_completo",
            Map,
            new Dictionary<string, object?> { ["@t"] = TenantContext.TenantId, ["@fid"] = funcionarioId },
            cancellationToken);

    private static Dependente MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        FuncionarioId = r.GetValueOrDefault<Guid>("funcionario_id"),
        NomeCompleto = r.GetValueOrDefault<string>("nome_completo") ?? string.Empty,
        Cpf = r.GetValueOrDefault<string>("cpf"),
        DataNascimento = DateOnly.FromDateTime(r.GetValueOrDefault<DateTime>("data_nascimento")),
        Tipo = Enum.TryParse<TipoDependente>(r.GetValueOrDefault<string>("tipo"), out var t) ? t : TipoDependente.Outro,
        Irrf = r.GetValueOrDefault<int>("irrf") == 1,
        SalarioFamilia = r.GetValueOrDefault<int>("salario_familia") == 1,
        PensaoAlimenticiaPct = r.GetValueOrDefault<decimal?>("pensao_alimenticia_pct"),
        DataInicio = r.GetValueOrDefault<DateTime?>("data_inicio") is { } di ? DateOnly.FromDateTime(di) : null,
        DataFim = r.GetValueOrDefault<DateTime?>("data_fim") is { } df ? DateOnly.FromDateTime(df) : null,
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by"),
    };
}
