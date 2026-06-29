using System.Data;
using Acme.Sistemas.Domain.Entities.Rh.Oficial671;
using Acme.Sistemas.Domain.Enums.Rh;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Rh.Oficial671;

public sealed class ExportacaoAfdRepository : BaseRepository<ExportacaoAfd>, IExportacaoAfdRepository
{
    public ExportacaoAfdRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "exportacoes_afd";
    protected override Func<IDataRecord, ExportacaoAfd> Map => MapEntity;

    public override Task AddAsync(ExportacaoAfd e, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO exportacoes_afd
                (id, tenant_id, empresa_id, periodo_inicio, periodo_fim, layout_versao,
                 arquivo_url, hash_sha256, status, gerado_em, erro, created_at, created_by)
            VALUES (@id, @t, @e, @ini, @fim, @lv, @url, @hash, @status, @ger, @err, @createdAt, @createdBy)",
            BuildParams(e), cancellationToken);

    public override Task UpdateAsync(ExportacaoAfd e, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE exportacoes_afd SET
                arquivo_url = @url, hash_sha256 = @hash, status = @status,
                gerado_em = @ger, erro = @err,
                updated_at = @updatedAt, updated_by = @updatedBy
            WHERE id = @id AND tenant_id = @t",
            BuildParams(e, updating: true), cancellationToken);

    public Task<IReadOnlyList<ExportacaoAfd>> ListByEmpresaAsync(Guid empresaId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(@"
            SELECT * FROM exportacoes_afd
            WHERE tenant_id = @t AND empresa_id = @e
            ORDER BY created_at DESC LIMIT 100",
            Map,
            new Dictionary<string, object?>
            {
                ["@t"] = TenantContext.TenantId,
                ["@e"] = empresaId,
            }, cancellationToken);

    private Dictionary<string, object?> BuildParams(ExportacaoAfd e, bool updating = false) => new()
    {
        ["@id"] = e.Id, ["@t"] = TenantContext.TenantId,
        ["@e"] = e.EmpresaId,
        ["@ini"] = e.PeriodoInicio.ToDateTime(TimeOnly.MinValue),
        ["@fim"] = e.PeriodoFim.ToDateTime(TimeOnly.MinValue),
        ["@lv"] = e.LayoutVersao,
        ["@url"] = e.ArquivoUrl, ["@hash"] = e.HashSha256,
        ["@status"] = e.Status.ToString(), ["@ger"] = e.GeradoEm, ["@err"] = e.Erro,
        ["@createdAt"] = e.CreatedAt, ["@createdBy"] = e.CreatedBy,
        ["@updatedAt"] = updating ? DateTime.UtcNow : (object?)null,
        ["@updatedBy"] = e.UpdatedBy,
    };

    private static ExportacaoAfd MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        EmpresaId = r.GetValueOrDefault<Guid>("empresa_id"),
        PeriodoInicio = DateOnly.FromDateTime(r.GetValueOrDefault<DateTime>("periodo_inicio")),
        PeriodoFim = DateOnly.FromDateTime(r.GetValueOrDefault<DateTime>("periodo_fim")),
        LayoutVersao = r.GetValueOrDefault<string>("layout_versao") ?? "003",
        ArquivoUrl = r.GetValueOrDefault<string>("arquivo_url"),
        HashSha256 = r.GetValueOrDefault<string>("hash_sha256"),
        Status = Enum.TryParse<StatusExportacao671>(r.GetValueOrDefault<string>("status"), out var s)
                 ? s : StatusExportacao671.Solicitada,
        GeradoEm = r.GetValueOrDefault<DateTime?>("gerado_em"),
        Erro = r.GetValueOrDefault<string>("erro"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
    };
}
