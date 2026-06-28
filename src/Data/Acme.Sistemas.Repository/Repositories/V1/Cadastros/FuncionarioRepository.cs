using System.Data;
using System.Text.Json;
using Acme.Sistemas.Domain.Entities.Cadastros;
using Acme.Sistemas.Domain.Entities.Rh;
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

    public override Task AddAsync(Funcionario f, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO funcionarios
            (id, tenant_id, nome_completo, cpf, email, telefone,
             cargo, departamento, centro_de_custo_id, data_admissao, data_demissao,
             usuario_id, status,
             cargo_id, lotacao_id, departamento_id, tipo_contrato, regime_remuneracao,
             codigo_matricula, pis, ctps, ctps_serie, ctps_uf,
             rg, rg_orgao, rg_uf, estado_civil, naturalidade, nacionalidade,
             endereco_json, conta_bancaria_json,
             created_at, created_by)
            VALUES
            (@id, @tenant_id, @nome, @cpf, @email, @telefone,
             @cargo, @depto, @ccid, @adm, @dem,
             @uid, @status,
             @cargoId, @lotacaoId, @departamentoId, @tipoContrato, @regime,
             @matricula, @pis, @ctps, @ctpsSerie, @ctpsUf,
             @rg, @rgOrgao, @rgUf, @estadoCivil, @naturalidade, @nacionalidade,
             @endereco, @conta,
             @created_at, @created_by)",
            BuildParams(f, includeId: true), cancellationToken);

    public override Task UpdateAsync(Funcionario f, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE funcionarios SET
                nome_completo = @nome, email = @email, telefone = @telefone,
                cargo = @cargo, departamento = @depto, centro_de_custo_id = @ccid,
                data_admissao = @adm, data_demissao = @dem,
                usuario_id = @uid, status = @status,
                cargo_id = @cargoId, lotacao_id = @lotacaoId, departamento_id = @departamentoId,
                tipo_contrato = @tipoContrato, regime_remuneracao = @regime,
                codigo_matricula = @matricula, pis = @pis, ctps = @ctps,
                ctps_serie = @ctpsSerie, ctps_uf = @ctpsUf,
                rg = @rg, rg_orgao = @rgOrgao, rg_uf = @rgUf,
                estado_civil = @estadoCivil, naturalidade = @naturalidade, nacionalidade = @nacionalidade,
                endereco_json = @endereco, conta_bancaria_json = @conta,
                updated_at = @updated_at, updated_by = @updated_by
            WHERE id = @id AND tenant_id = @tenant_id AND deleted_at IS NULL",
            BuildParams(f, includeId: true, isUpdate: true), cancellationToken);

    public Task<Funcionario?> GetByCpfAsync(string cpf, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(
            "SELECT * FROM funcionarios WHERE tenant_id = @tenantId AND cpf = @cpf AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@cpf"] = cpf },
            cancellationToken);

    public Task<Funcionario?> GetByMatriculaAsync(string matricula, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(
            "SELECT * FROM funcionarios WHERE tenant_id = @tenantId AND codigo_matricula = @m AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = TenantContext.TenantId, ["@m"] = matricula },
            cancellationToken);

    private Dictionary<string, object?> BuildParams(Funcionario f, bool includeId, bool isUpdate = false)
    {
        var enderecoJson = f.EnderecoJson ?? (f.Endereco is null ? null : JsonSerializer.Serialize(f.Endereco));
        var contaJson = f.ContaBancariaJson ?? (f.ContaBancaria is null ? null : JsonSerializer.Serialize(f.ContaBancaria));

        var dict = new Dictionary<string, object?>
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
            ["@cargoId"] = f.CargoId,
            ["@lotacaoId"] = f.LotacaoId,
            ["@departamentoId"] = f.DepartamentoId,
            ["@tipoContrato"] = f.TipoContrato?.ToString(),
            ["@regime"] = f.RegimeRemuneracao?.ToString(),
            ["@matricula"] = f.CodigoMatricula,
            ["@pis"] = f.Pis,
            ["@ctps"] = f.Ctps,
            ["@ctpsSerie"] = f.CtpsSerie,
            ["@ctpsUf"] = f.CtpsUf,
            ["@rg"] = f.Rg,
            ["@rgOrgao"] = f.RgOrgao,
            ["@rgUf"] = f.RgUf,
            ["@estadoCivil"] = f.EstadoCivil?.ToString(),
            ["@naturalidade"] = f.Naturalidade,
            ["@nacionalidade"] = f.Nacionalidade,
            ["@endereco"] = enderecoJson,
            ["@conta"] = contaJson,
        };

        if (isUpdate)
        {
            dict["@updated_at"] = DateTime.UtcNow;
            dict["@updated_by"] = f.UpdatedBy;
        }
        else
        {
            dict["@created_at"] = f.CreatedAt;
            dict["@created_by"] = f.CreatedBy;
        }
        return dict;
    }

    private static Funcionario MapEntity(IDataRecord r)
    {
        var endJson = r.GetValueOrDefault<string>("endereco_json");
        var contaJson = r.GetValueOrDefault<string>("conta_bancaria_json");

        return new()
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
            CargoId = r.GetValueOrDefault<Guid?>("cargo_id"),
            LotacaoId = r.GetValueOrDefault<Guid?>("lotacao_id"),
            DepartamentoId = r.GetValueOrDefault<Guid?>("departamento_id"),
            TipoContrato = ParseEnumOrNull<TipoContrato>(r.GetValueOrDefault<string>("tipo_contrato")),
            RegimeRemuneracao = ParseEnumOrNull<RegimeRemuneracao>(r.GetValueOrDefault<string>("regime_remuneracao")),
            CodigoMatricula = r.GetValueOrDefault<string>("codigo_matricula"),
            Pis = r.GetValueOrDefault<string>("pis"),
            Ctps = r.GetValueOrDefault<string>("ctps"),
            CtpsSerie = r.GetValueOrDefault<string>("ctps_serie"),
            CtpsUf = r.GetValueOrDefault<string>("ctps_uf"),
            Rg = r.GetValueOrDefault<string>("rg"),
            RgOrgao = r.GetValueOrDefault<string>("rg_orgao"),
            RgUf = r.GetValueOrDefault<string>("rg_uf"),
            EstadoCivil = ParseEnumOrNull<EstadoCivil>(r.GetValueOrDefault<string>("estado_civil")),
            Naturalidade = r.GetValueOrDefault<string>("naturalidade"),
            Nacionalidade = r.GetValueOrDefault<string>("nacionalidade"),
            EnderecoJson = endJson,
            ContaBancariaJson = contaJson,
            Endereco = string.IsNullOrWhiteSpace(endJson) ? null : TryDeserialize<EnderecoFuncionario>(endJson),
            ContaBancaria = string.IsNullOrWhiteSpace(contaJson) ? null : TryDeserialize<ContaBancariaFuncionario>(contaJson),
            CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
            CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
            UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
            UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
            DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
            DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by"),
        };
    }

    private static TEnum? ParseEnumOrNull<TEnum>(string? value) where TEnum : struct
        => string.IsNullOrWhiteSpace(value) ? null : Enum.TryParse<TEnum>(value, out var v) ? v : null;

    private static T? TryDeserialize<T>(string json) where T : class
    {
        try { return JsonSerializer.Deserialize<T>(json); }
        catch (JsonException) { return null; }
    }
}
