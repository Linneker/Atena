using System.Data;
using Acme.Sistemas.Domain.Entities.Rh.Oficial671;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Rh.Oficial671;

public sealed class ComprovantePontoRepository : BaseRepository<ComprovantePonto>, IComprovantePontoRepository
{
    public ComprovantePontoRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "comprovantes_ponto";
    protected override Func<IDataRecord, ComprovantePonto> Map => MapEntity;

    public override Task AddAsync(ComprovantePonto c, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO comprovantes_ponto
                (id, tenant_id, empresa_id, marcacao_id, nsr, payload_texto, assinatura_base64,
                 hash_sha256, certificado_thumbprint, emitido_em, created_at, created_by)
            VALUES (@id, @t, @e, @m, @nsr, @payload, @sig, @hash, @thumb, @emit, @createdAt, @createdBy)",
            new Dictionary<string, object?>
            {
                ["@id"] = c.Id, ["@t"] = TenantContext.TenantId,
                ["@e"] = c.EmpresaId, ["@m"] = c.MarcacaoId, ["@nsr"] = c.Nsr,
                ["@payload"] = c.PayloadTexto, ["@sig"] = c.AssinaturaBase64,
                ["@hash"] = c.HashSha256, ["@thumb"] = c.CertificadoThumbprint,
                ["@emit"] = c.EmitidoEm,
                ["@createdAt"] = c.CreatedAt, ["@createdBy"] = c.CreatedBy,
            }, cancellationToken);

    public override Task UpdateAsync(ComprovantePonto c, CancellationToken cancellationToken = default)
        => throw new NotSupportedException("Comprovantes são imutáveis — não atualizar.");

    public Task<ComprovantePonto?> GetByMarcacaoAsync(Guid marcacaoId, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(@"
            SELECT * FROM comprovantes_ponto
            WHERE tenant_id = @t AND marcacao_id = @m AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?>
            {
                ["@t"] = TenantContext.TenantId,
                ["@m"] = marcacaoId,
            }, cancellationToken);

    public Task<IReadOnlyList<ComprovantePonto>> ListByEmpresaPeriodoAsync(
        Guid empresaId, DateTime inicio, DateTime fim, CancellationToken cancellationToken = default)
        => Db.QueryAsync(@"
            SELECT * FROM comprovantes_ponto
            WHERE tenant_id = @t AND empresa_id = @e AND deleted_at IS NULL
              AND emitido_em >= @ini AND emitido_em < @fim
            ORDER BY nsr",
            Map,
            new Dictionary<string, object?>
            {
                ["@t"] = TenantContext.TenantId, ["@e"] = empresaId,
                ["@ini"] = inicio, ["@fim"] = fim,
            }, cancellationToken);

    private static ComprovantePonto MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        EmpresaId = r.GetValueOrDefault<Guid>("empresa_id"),
        MarcacaoId = r.GetValueOrDefault<Guid>("marcacao_id"),
        Nsr = r.GetValueOrDefault<long>("nsr"),
        PayloadTexto = r.GetValueOrDefault<string>("payload_texto") ?? string.Empty,
        AssinaturaBase64 = r.GetValueOrDefault<string>("assinatura_base64") ?? string.Empty,
        HashSha256 = r.GetValueOrDefault<string>("hash_sha256") ?? string.Empty,
        CertificadoThumbprint = r.GetValueOrDefault<string>("certificado_thumbprint"),
        EmitidoEm = r.GetValueOrDefault<DateTime>("emitido_em"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by"),
    };
}
