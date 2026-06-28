using System.Data;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Rh;

public sealed class AjustePontoRepository : BaseRepository<AjustePonto>, IAjustePontoRepository
{
    public AjustePontoRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "ajustes_ponto";
    protected override Func<IDataRecord, AjustePonto> Map => MapEntity;

    public override Task AddAsync(AjustePonto a, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO ajustes_ponto
                (id, tenant_id, funcionario_id, marcacao_original_id, tipo_ajuste,
                 data_hora_proposta, tipo_marcacao_proposta, motivo, anexo_url, status,
                 aprovador_id, decisao_em, justificativa_decisao, marcacao_resultante_id,
                 created_at, created_by)
            VALUES (@id, @t, @fid, @moid, @tipo, @dhp, @tmp, @motivo, @anexo, @status,
                    @apid, @dec, @just, @mrid, @createdAt, @createdBy)",
            BuildParams(a), cancellationToken);

    public override Task UpdateAsync(AjustePonto a, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE ajustes_ponto SET
                status = @status, aprovador_id = @apid, decisao_em = @dec,
                justificativa_decisao = @just, marcacao_resultante_id = @mrid,
                updated_at = @updatedAt, updated_by = @updatedBy
            WHERE id = @id AND tenant_id = @t AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@id"] = a.Id,
                ["@t"] = TenantContext.TenantId,
                ["@status"] = a.Status.ToString(),
                ["@apid"] = a.AprovadorId,
                ["@dec"] = a.DecisaoEm,
                ["@just"] = a.JustificativaDecisao,
                ["@mrid"] = a.MarcacaoResultanteId,
                ["@updatedAt"] = DateTime.UtcNow,
                ["@updatedBy"] = a.UpdatedBy,
            }, cancellationToken);

    public Task<IReadOnlyList<AjustePonto>> ListarPendentesAsync(int skip, int take, CancellationToken cancellationToken = default)
        => Db.QueryAsync(@"
            SELECT * FROM ajustes_ponto
            WHERE tenant_id = @t AND status = 'Pendente' AND deleted_at IS NULL
            ORDER BY created_at LIMIT @take OFFSET @skip",
            Map,
            new Dictionary<string, object?>
            {
                ["@t"] = TenantContext.TenantId,
                ["@skip"] = skip,
                ["@take"] = take,
            }, cancellationToken);

    public Task<long> CountPendentesAsync(CancellationToken cancellationToken = default)
        => Db.ExecuteScalarAsync<long>(@"
            SELECT COUNT(*) FROM ajustes_ponto
            WHERE tenant_id = @t AND status = 'Pendente' AND deleted_at IS NULL",
            new Dictionary<string, object?> { ["@t"] = TenantContext.TenantId },
            cancellationToken);

    private Dictionary<string, object?> BuildParams(AjustePonto a) => new()
    {
        ["@id"] = a.Id,
        ["@t"] = TenantContext.TenantId,
        ["@fid"] = a.FuncionarioId,
        ["@moid"] = a.MarcacaoOriginalId,
        ["@tipo"] = a.TipoAjuste.ToString(),
        ["@dhp"] = a.DataHoraProposta,
        ["@tmp"] = a.TipoMarcacaoProposta?.ToString(),
        ["@motivo"] = a.Motivo,
        ["@anexo"] = a.AnexoUrl,
        ["@status"] = a.Status.ToString(),
        ["@apid"] = a.AprovadorId,
        ["@dec"] = a.DecisaoEm,
        ["@just"] = a.JustificativaDecisao,
        ["@mrid"] = a.MarcacaoResultanteId,
        ["@createdAt"] = a.CreatedAt,
        ["@createdBy"] = a.CreatedBy,
    };

    private static AjustePonto MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        FuncionarioId = r.GetValueOrDefault<Guid>("funcionario_id"),
        MarcacaoOriginalId = r.GetValueOrDefault<Guid?>("marcacao_original_id"),
        TipoAjuste = Enum.TryParse<TipoAjuste>(r.GetValueOrDefault<string>("tipo_ajuste"), out var t) ? t : TipoAjuste.Justificativa,
        DataHoraProposta = r.GetValueOrDefault<DateTime?>("data_hora_proposta"),
        TipoMarcacaoProposta = Enum.TryParse<TipoMarcacao>(r.GetValueOrDefault<string>("tipo_marcacao_proposta"), out var tm) ? tm : null,
        Motivo = r.GetValueOrDefault<string>("motivo") ?? string.Empty,
        AnexoUrl = r.GetValueOrDefault<string>("anexo_url"),
        Status = Enum.TryParse<StatusAjuste>(r.GetValueOrDefault<string>("status"), out var s) ? s : StatusAjuste.Pendente,
        AprovadorId = r.GetValueOrDefault<Guid?>("aprovador_id"),
        DecisaoEm = r.GetValueOrDefault<DateTime?>("decisao_em"),
        JustificativaDecisao = r.GetValueOrDefault<string>("justificativa_decisao"),
        MarcacaoResultanteId = r.GetValueOrDefault<Guid?>("marcacao_resultante_id"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by"),
    };
}
