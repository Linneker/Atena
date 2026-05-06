namespace Acme.Sistemas.Repository.Repositories.V1.Tenants;

internal static class TenantQuery
{
    public const string SelectColumns = @"id, razao_social, cnpj, plano, status, logo_url, cor_primaria, fuso_horario,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public const string GetById = $@"
        SELECT {SelectColumns} FROM tenants
        WHERE id = @id AND deleted_at IS NULL LIMIT 1";

    public const string GetByCnpj = $@"
        SELECT {SelectColumns} FROM tenants
        WHERE cnpj = @cnpj AND deleted_at IS NULL LIMIT 1";

    public const string List = $@"
        SELECT {SelectColumns} FROM tenants
        WHERE deleted_at IS NULL
        ORDER BY created_at DESC
        LIMIT @take OFFSET @skip";

    public const string Insert = @"
        INSERT INTO tenants
        (id, razao_social, cnpj, plano, status, logo_url, cor_primaria, fuso_horario, created_at, created_by)
        VALUES
        (@id, @razao_social, @cnpj, @plano, @status, @logo_url, @cor_primaria, @fuso_horario, @created_at, @created_by)";

    public const string Update = @"
        UPDATE tenants SET
            razao_social = @razao_social,
            plano = @plano,
            status = @status,
            logo_url = @logo_url,
            cor_primaria = @cor_primaria,
            fuso_horario = @fuso_horario,
            updated_at = @updated_at,
            updated_by = @updated_by
        WHERE id = @id";

    public const string SoftDelete = @"
        UPDATE tenants SET deleted_at = @deleted_at, deleted_by = @deleted_by
        WHERE id = @id";

    public const string GetLimites = @"
        SELECT tenant_id, max_usuarios, max_nfe_mes, max_storage_gb, updated_at
        FROM tenant_limites WHERE tenant_id = @tenant_id LIMIT 1";

    public const string UpsertLimites = @"
        INSERT INTO tenant_limites (tenant_id, max_usuarios, max_nfe_mes, max_storage_gb, updated_at)
        VALUES (@tenant_id, @max_usuarios, @max_nfe_mes, @max_storage_gb, @updated_at)
        ON DUPLICATE KEY UPDATE
            max_usuarios = @max_usuarios,
            max_nfe_mes = @max_nfe_mes,
            max_storage_gb = @max_storage_gb,
            updated_at = @updated_at";
}
