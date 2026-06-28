using System.Data;
using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Rh;

public sealed class DispositivoMobileRepository : BaseRepository<DispositivoMobile>, IDispositivoMobileRepository
{
    public DispositivoMobileRepository(IDataConfiguration db, ITenantContext tenantContext)
        : base(db, tenantContext) { }

    protected override string TableName => "dispositivos_mobile";
    protected override Func<IDataRecord, DispositivoMobile> Map => MapEntity;

    public override Task AddAsync(DispositivoMobile d, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            INSERT INTO dispositivos_mobile
                (id, tenant_id, funcionario_id, usuario_id, device_id, plataforma, modelo,
                 os_version, app_version, push_token, chave_publica_local, ativo,
                 registrado_em, ultimo_acesso, created_at, created_by)
            VALUES (@id, @t, @fid, @uid, @dev, @plat, @mod, @osv, @appv, @push, @chave, @ativo,
                    @reg, @ua, @createdAt, @createdBy)",
            new Dictionary<string, object?>
            {
                ["@id"] = d.Id, ["@t"] = TenantContext.TenantId,
                ["@fid"] = d.FuncionarioId, ["@uid"] = d.UsuarioId,
                ["@dev"] = d.DeviceId, ["@plat"] = d.Plataforma.ToString(),
                ["@mod"] = d.Modelo, ["@osv"] = d.OsVersion, ["@appv"] = d.AppVersion,
                ["@push"] = d.PushToken, ["@chave"] = d.ChavePublicaLocal,
                ["@ativo"] = d.Ativo ? 1 : 0,
                ["@reg"] = d.RegistradoEm, ["@ua"] = d.UltimoAcesso,
                ["@createdAt"] = d.CreatedAt, ["@createdBy"] = d.CreatedBy,
            }, cancellationToken);

    public override Task UpdateAsync(DispositivoMobile d, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE dispositivos_mobile SET
                modelo = @mod, os_version = @osv, app_version = @appv, push_token = @push,
                chave_publica_local = @chave, ativo = @ativo, ultimo_acesso = @ua,
                updated_at = @updatedAt, updated_by = @updatedBy
            WHERE id = @id AND tenant_id = @t",
            new Dictionary<string, object?>
            {
                ["@id"] = d.Id, ["@t"] = TenantContext.TenantId,
                ["@mod"] = d.Modelo, ["@osv"] = d.OsVersion, ["@appv"] = d.AppVersion,
                ["@push"] = d.PushToken, ["@chave"] = d.ChavePublicaLocal,
                ["@ativo"] = d.Ativo ? 1 : 0, ["@ua"] = d.UltimoAcesso,
                ["@updatedAt"] = DateTime.UtcNow, ["@updatedBy"] = d.UpdatedBy,
            }, cancellationToken);

    public Task<DispositivoMobile?> GetByDeviceIdAsync(Guid usuarioId, string deviceId, CancellationToken cancellationToken = default)
        => Db.QueryFirstOrDefaultAsync(@"
            SELECT * FROM dispositivos_mobile
            WHERE tenant_id = @t AND usuario_id = @u AND device_id = @d LIMIT 1",
            Map,
            new Dictionary<string, object?>
            {
                ["@t"] = TenantContext.TenantId, ["@u"] = usuarioId, ["@d"] = deviceId,
            }, cancellationToken);

    public Task<IReadOnlyList<DispositivoMobile>> ListByUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default)
        => Db.QueryAsync(@"
            SELECT * FROM dispositivos_mobile
            WHERE tenant_id = @t AND usuario_id = @u ORDER BY registrado_em DESC",
            Map,
            new Dictionary<string, object?> { ["@t"] = TenantContext.TenantId, ["@u"] = usuarioId },
            cancellationToken);

    public Task<IReadOnlyList<DispositivoMobile>> ListAllTenantAsync(int skip, int take, CancellationToken cancellationToken = default)
        => Db.QueryAsync(@"
            SELECT * FROM dispositivos_mobile
            WHERE tenant_id = @t
            ORDER BY registrado_em DESC
            LIMIT @take OFFSET @skip",
            Map,
            new Dictionary<string, object?>
            {
                ["@t"] = TenantContext.TenantId, ["@skip"] = skip, ["@take"] = take,
            }, cancellationToken);

    public Task<long> CountTenantAsync(CancellationToken cancellationToken = default)
        => Db.ExecuteScalarAsync<long>(
            "SELECT COUNT(*) FROM dispositivos_mobile WHERE tenant_id = @t",
            new Dictionary<string, object?> { ["@t"] = TenantContext.TenantId },
            cancellationToken);

    public Task RevogarAsync(Guid id, Guid revogadoPor, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE dispositivos_mobile SET
                ativo = 0, revogado_em = @r, revogado_por = @por,
                updated_at = @updatedAt, updated_by = @updatedBy
            WHERE id = @id AND tenant_id = @t",
            new Dictionary<string, object?>
            {
                ["@id"] = id, ["@t"] = TenantContext.TenantId,
                ["@r"] = DateTime.UtcNow, ["@por"] = revogadoPor,
                ["@updatedAt"] = DateTime.UtcNow, ["@updatedBy"] = revogadoPor,
            }, cancellationToken);

    public Task RegistrarUltimoAcessoAsync(Guid id, CancellationToken cancellationToken = default)
        => Db.ExecuteAsync(@"
            UPDATE dispositivos_mobile SET ultimo_acesso = @now
            WHERE id = @id AND tenant_id = @t",
            new Dictionary<string, object?>
            {
                ["@id"] = id, ["@t"] = TenantContext.TenantId, ["@now"] = DateTime.UtcNow,
            }, cancellationToken);

    private static DispositivoMobile MapEntity(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        FuncionarioId = r.GetValueOrDefault<Guid?>("funcionario_id"),
        UsuarioId = r.GetValueOrDefault<Guid>("usuario_id"),
        DeviceId = r.GetValueOrDefault<string>("device_id") ?? string.Empty,
        Plataforma = Enum.TryParse<PlataformaMobile>(r.GetValueOrDefault<string>("plataforma"), out var p) ? p : PlataformaMobile.Android,
        Modelo = r.GetValueOrDefault<string>("modelo"),
        OsVersion = r.GetValueOrDefault<string>("os_version"),
        AppVersion = r.GetValueOrDefault<string>("app_version"),
        PushToken = r.GetValueOrDefault<string>("push_token"),
        ChavePublicaLocal = r.GetValueOrDefault<string>("chave_publica_local"),
        Ativo = r.GetValueOrDefault<int>("ativo") == 1,
        RevogadoEm = r.GetValueOrDefault<DateTime?>("revogado_em"),
        RevogadoPor = r.GetValueOrDefault<Guid?>("revogado_por"),
        RegistradoEm = r.GetValueOrDefault<DateTime>("registrado_em"),
        UltimoAcesso = r.GetValueOrDefault<DateTime?>("ultimo_acesso"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
    };
}
