using System.Data;
using Acme.Sistemas.Domain.Entities.Tenants;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Tenants;

public sealed class TenantRepository : ITenantRepository
{
    private readonly IDataConfiguration _db;

    public TenantRepository(IDataConfiguration db)
    {
        _db = db;
    }

    public Task<Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _db.QueryFirstOrDefaultAsync(TenantQuery.GetById, Map,
            new Dictionary<string, object?> { ["@id"] = id }, cancellationToken);

    public Task<Tenant?> GetByCnpjAsync(string cnpj, CancellationToken cancellationToken = default)
        => _db.QueryFirstOrDefaultAsync(TenantQuery.GetByCnpj, Map,
            new Dictionary<string, object?> { ["@cnpj"] = cnpj }, cancellationToken);

    public Task<IReadOnlyList<Tenant>> ListAsync(int skip = 0, int take = 50, CancellationToken cancellationToken = default)
        => _db.QueryAsync(TenantQuery.List, Map,
            new Dictionary<string, object?> { ["@skip"] = skip, ["@take"] = take }, cancellationToken);

    public async Task AddAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, object?>
        {
            ["@id"] = tenant.Id,
            ["@razao_social"] = tenant.RazaoSocial,
            ["@cnpj"] = tenant.Cnpj,
            ["@plano"] = tenant.Plano,
            ["@status"] = (int)tenant.Status,
            ["@logo_url"] = tenant.LogoUrl,
            ["@cor_primaria"] = tenant.CorPrimaria,
            ["@fuso_horario"] = tenant.FusoHorario,
            ["@created_at"] = tenant.CreatedAt,
            ["@created_by"] = tenant.CreatedBy
        };
        await _db.ExecuteAsync(TenantQuery.Insert, parameters, cancellationToken);
    }

    public async Task UpdateAsync(Tenant tenant, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, object?>
        {
            ["@id"] = tenant.Id,
            ["@razao_social"] = tenant.RazaoSocial,
            ["@plano"] = tenant.Plano,
            ["@status"] = (int)tenant.Status,
            ["@logo_url"] = tenant.LogoUrl,
            ["@cor_primaria"] = tenant.CorPrimaria,
            ["@fuso_horario"] = tenant.FusoHorario,
            ["@updated_at"] = DateTime.UtcNow,
            ["@updated_by"] = tenant.UpdatedBy
        };
        await _db.ExecuteAsync(TenantQuery.Update, parameters, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, Guid deletedBy, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, object?>
        {
            ["@id"] = id,
            ["@deleted_at"] = DateTime.UtcNow,
            ["@deleted_by"] = deletedBy
        };
        await _db.ExecuteAsync(TenantQuery.SoftDelete, parameters, cancellationToken);
    }

    public Task<TenantLimites?> GetLimitesAsync(Guid tenantId, CancellationToken cancellationToken = default)
        => _db.QueryFirstOrDefaultAsync(TenantQuery.GetLimites, MapLimites,
            new Dictionary<string, object?> { ["@tenant_id"] = tenantId }, cancellationToken);

    public async Task UpsertLimitesAsync(TenantLimites limites, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, object?>
        {
            ["@tenant_id"] = limites.TenantId,
            ["@max_usuarios"] = limites.MaxUsuarios,
            ["@max_nfe_mes"] = limites.MaxNFeMes,
            ["@max_storage_gb"] = limites.MaxStorageGb,
            ["@updated_at"] = DateTime.UtcNow
        };
        await _db.ExecuteAsync(TenantQuery.UpsertLimites, parameters, cancellationToken);
    }

    private static Tenant Map(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        RazaoSocial = r.GetValueOrDefault<string>("razao_social") ?? string.Empty,
        Cnpj = r.GetValueOrDefault<string>("cnpj") ?? string.Empty,
        Plano = r.GetValueOrDefault<string>("plano") ?? string.Empty,
        Status = (StatusAtivo)r.GetValueOrDefault<int>("status"),
        LogoUrl = r.GetValueOrDefault<string>("logo_url"),
        CorPrimaria = r.GetValueOrDefault<string>("cor_primaria"),
        FusoHorario = r.GetValueOrDefault<string>("fuso_horario") ?? "America/Sao_Paulo",
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };

    private static TenantLimites MapLimites(IDataRecord r) => new()
    {
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        MaxUsuarios = r.GetValueOrDefault<int>("max_usuarios"),
        MaxNFeMes = r.GetValueOrDefault<int>("max_nfe_mes"),
        MaxStorageGb = r.GetValueOrDefault<int>("max_storage_gb"),
        UpdatedAt = r.GetValueOrDefault<DateTime>("updated_at")
    };
}
