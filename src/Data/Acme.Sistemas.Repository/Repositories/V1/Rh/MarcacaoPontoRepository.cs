using System.Data;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Rh;

public sealed class MarcacaoPontoRepository : BaseRepository<MarcacaoPonto>, IMarcacaoPontoRepository
{
    public MarcacaoPontoRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "marcacoes_ponto";
    protected override Func<IDataRecord, MarcacaoPonto> Map => MapEntity;

    public override Task AddAsync(MarcacaoPonto m, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO marcacoes_ponto
                (id, tenant_id, funcionario_id, tipo, data_hora, origem, latitude, longitude,
                 ip_origem, user_agent, device_id, foto_url, hash_anterior, hash_integridade,
                 status, marcacao_origem_id, created_at, created_by)
            VALUES (@id, @t, @fid, @tipo, @dh, @origem, @lat, @lng,
                    @ip, @ua, @did, @foto, @hashAnt, @hash,
                    @status, @origemId, @createdAt, @createdBy)",
            BuildParams(m, isUpdate: false), cancellationToken);

    public override Task UpdateAsync(MarcacaoPonto m, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE marcacoes_ponto SET
                status = @status, updated_at = @updatedAt, updated_by = @updatedBy
            WHERE id = @id AND tenant_id = @t AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = m.Id,
                ["@t"] = TenantContext.TenantId,
                ["@status"] = m.Status.ToString(),
                ["@updatedAt"] = DateTime.UtcNow,
                ["@updatedBy"] = m.UpdatedBy,
            }, cancellationToken);

    public Task<MarcacaoPonto?> GetUltimaPorFuncionarioAsync(
        Guid funcionarioId, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(@"
            SELECT * FROM marcacoes_ponto
            WHERE tenant_id = @t AND funcionario_id = @fid AND deleted_at IS NULL
            ORDER BY data_hora DESC, created_at DESC LIMIT 1",
            Map,
            new Dictionary<string, object?>
            {
                ["@t"] = TenantContext.TenantId,
                ["@fid"] = funcionarioId,
            }, cancellationToken);

    public Task<IReadOnlyList<MarcacaoPonto>> ListByFuncionarioPeriodoAsync(
        Guid funcionarioId, DateOnly inicio, DateOnly fim, CancellationToken cancellationToken = default)
        => Db.QueryAsync(@"
            SELECT * FROM marcacoes_ponto
            WHERE tenant_id = @t AND funcionario_id = @fid AND deleted_at IS NULL
              AND data_hora >= @ini AND data_hora < @fim
            ORDER BY data_hora",
            Map,
            new Dictionary<string, object?>
            {
                ["@t"] = TenantContext.TenantId,
                ["@fid"] = funcionarioId,
                ["@ini"] = inicio.ToDateTime(TimeOnly.MinValue),
                ["@fim"] = fim.AddDays(1).ToDateTime(TimeOnly.MinValue),
            }, cancellationToken);

    public Task<IReadOnlyList<MarcacaoPonto>> ListAllByFuncionarioOrdenadasAsync(
        Guid funcionarioId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(@"
            SELECT * FROM marcacoes_ponto
            WHERE tenant_id = @t AND funcionario_id = @fid AND deleted_at IS NULL
            ORDER BY data_hora, created_at",
            Map,
            new Dictionary<string, object?>
            {
                ["@t"] = TenantContext.TenantId,
                ["@fid"] = funcionarioId,
            }, cancellationToken);

    public Task<IReadOnlyList<Guid>> ListFuncionarioIdsComMarcacoesAsync(
        CancellationToken cancellationToken = default)
        => Db.QueryAsync(@"
            SELECT DISTINCT funcionario_id FROM marcacoes_ponto
            WHERE tenant_id = @t AND deleted_at IS NULL",
            r => r.GetValueOrDefault<Guid>("funcionario_id"),
            new Dictionary<string, object?> { ["@t"] = TenantContext.TenantId },
            cancellationToken);

    private Dictionary<string, object?> BuildParams(MarcacaoPonto m, bool isUpdate) => new()
    {
        ["@id"] = m.Id,
        ["@t"] = TenantContext.TenantId,
        ["@fid"] = m.FuncionarioId,
        ["@tipo"] = m.Tipo.ToString(),
        ["@dh"] = m.DataHora,
        ["@origem"] = m.Origem.ToString(),
        ["@lat"] = m.Latitude,
        ["@lng"] = m.Longitude,
        ["@ip"] = m.IpOrigem,
        ["@ua"] = m.UserAgent,
        ["@did"] = m.DeviceId,
        ["@foto"] = m.FotoUrl,
        ["@hashAnt"] = m.HashAnterior,
        ["@hash"] = m.HashIntegridade,
        ["@status"] = m.Status.ToString(),
        ["@origemId"] = m.MarcacaoOrigemId,
        ["@createdAt"] = m.CreatedAt,
        ["@createdBy"] = m.CreatedBy,
    };

    private static MarcacaoPonto MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        FuncionarioId = r.GetValueOrDefault<Guid>("funcionario_id"),
        Tipo = Enum.TryParse<TipoMarcacao>(r.GetValueOrDefault<string>("tipo"), out var t) ? t : TipoMarcacao.Entrada,
        DataHora = r.GetValueOrDefault<DateTime>("data_hora"),
        Origem = Enum.TryParse<OrigemMarcacao>(r.GetValueOrDefault<string>("origem"), out var o) ? o : OrigemMarcacao.Web,
        Latitude = r.GetValueOrDefault<decimal?>("latitude"),
        Longitude = r.GetValueOrDefault<decimal?>("longitude"),
        IpOrigem = r.GetValueOrDefault<string>("ip_origem"),
        UserAgent = r.GetValueOrDefault<string>("user_agent"),
        DeviceId = r.GetValueOrDefault<string>("device_id"),
        FotoUrl = r.GetValueOrDefault<string>("foto_url"),
        HashAnterior = r.GetValueOrDefault<string>("hash_anterior"),
        HashIntegridade = r.GetValueOrDefault<string>("hash_integridade") ?? string.Empty,
        Status = Enum.TryParse<StatusMarcacao>(r.GetValueOrDefault<string>("status"), out var s) ? s : StatusMarcacao.Valida,
        MarcacaoOrigemId = r.GetValueOrDefault<Guid?>("marcacao_origem_id"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by"),
    };
}
