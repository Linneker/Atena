using System.Data;
using Acme.Sistemas.Domain.Entities.Users;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Users;

public sealed class UsuarioRepository : IUsuarioRepository
{
    private readonly IDataConfiguration _db;
    private readonly ITenantContext _tenantContext;

    private const string Cols = @"id, tenant_id, nome_completo, email, password_hash, status,
        failed_login_attempts, locked_until, last_login_at,
        email_confirmed_at, email_confirmation_token_hash, email_confirmation_expires_at,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public UsuarioRepository(IDataConfiguration db, ITenantContext tenantContext)
    {
        _db = db;
        _tenantContext = tenantContext;
    }

    public Task<Usuario?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => _db.QueryFirstOrDefaultAsync(
            $"SELECT {Cols} FROM usuarios WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@id"] = id, ["@tenantId"] = _tenantContext.TenantId },
            cancellationToken);

    public Task<Usuario?> GetByIdAcrossTenantsAsync(Guid id, CancellationToken cancellationToken = default)
        => _db.QueryFirstOrDefaultAsync(
            $"SELECT {Cols} FROM usuarios WHERE id = @id AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@id"] = id },
            cancellationToken);

    public Task<Usuario?> GetByEmailAsync(Guid tenantId, string email, CancellationToken cancellationToken = default)
        => _db.QueryFirstOrDefaultAsync(
            $"SELECT {Cols} FROM usuarios WHERE tenant_id = @tenantId AND email = @email AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = tenantId, ["@email"] = email },
            cancellationToken);

    public Task<Usuario?> GetByEmailAcrossTenantsAsync(string email, CancellationToken cancellationToken = default)
        => _db.QueryFirstOrDefaultAsync(
            $"SELECT {Cols} FROM usuarios WHERE email = @email AND deleted_at IS NULL ORDER BY created_at DESC LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@email"] = email },
            cancellationToken);

    public Task<Usuario?> GetByConfirmationTokenAsync(string tokenHash, CancellationToken cancellationToken = default)
        => _db.QueryFirstOrDefaultAsync(
            $@"SELECT {Cols} FROM usuarios
               WHERE email_confirmation_token_hash = @hash AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@hash"] = tokenHash },
            cancellationToken);

    public Task<IReadOnlyList<Usuario>> ListAsync(int skip = 0, int take = 50, CancellationToken cancellationToken = default)
        => _db.QueryAsync(
            $"SELECT {Cols} FROM usuarios WHERE tenant_id = @tenantId AND deleted_at IS NULL ORDER BY nome_completo LIMIT @take OFFSET @skip",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = _tenantContext.TenantId, ["@skip"] = skip, ["@take"] = take },
            cancellationToken);

    public Task AddAsync(Usuario u, CancellationToken cancellationToken = default)
        => _db.ExecuteAsync(
            @"INSERT INTO usuarios (id, tenant_id, nome_completo, email, password_hash, status,
              email_confirmed_at, email_confirmation_token_hash, email_confirmation_expires_at,
              created_at, created_by)
              VALUES (@id, @tenant_id, @nome, @email, @hash, @status,
              @email_confirmed_at, @email_confirmation_token_hash, @email_confirmation_expires_at,
              @created_at, @created_by)",
            new Dictionary<string, object?>
            {
                ["@id"] = u.Id,
                ["@tenant_id"] = u.TenantId,
                ["@nome"] = u.NomeCompleto,
                ["@email"] = u.Email,
                ["@hash"] = u.PasswordHash,
                ["@status"] = (int)u.Status,
                ["@email_confirmed_at"] = u.EmailConfirmedAt,
                ["@email_confirmation_token_hash"] = u.EmailConfirmationTokenHash,
                ["@email_confirmation_expires_at"] = u.EmailConfirmationExpiresAt,
                ["@created_at"] = u.CreatedAt,
                ["@created_by"] = u.CreatedBy
            }, cancellationToken);

    public Task UpdateAsync(Usuario u, CancellationToken cancellationToken = default)
        => _db.ExecuteAsync(
            @"UPDATE usuarios SET nome_completo = @nome, email = @email, status = @status,
              updated_at = @updated_at, updated_by = @updated_by
              WHERE id = @id AND tenant_id = @tenant_id",
            new Dictionary<string, object?>
            {
                ["@id"] = u.Id,
                ["@tenant_id"] = _tenantContext.TenantId,
                ["@nome"] = u.NomeCompleto,
                ["@email"] = u.Email,
                ["@status"] = (int)u.Status,
                ["@updated_at"] = DateTime.UtcNow,
                ["@updated_by"] = u.UpdatedBy
            }, cancellationToken);

    public Task UpdateLoginStatusAsync(Guid id, int failedAttempts, DateTime? lockedUntil, DateTime? lastLoginAt, CancellationToken cancellationToken = default)
        => _db.ExecuteAsync(
            @"UPDATE usuarios SET failed_login_attempts = @attempts, locked_until = @locked, last_login_at = @last
              WHERE id = @id",
            new Dictionary<string, object?>
            {
                ["@id"] = id,
                ["@attempts"] = failedAttempts,
                ["@locked"] = lockedUntil,
                ["@last"] = lastLoginAt
            }, cancellationToken);

    public Task SetEmailConfirmationTokenAsync(Guid id, string tokenHash, DateTime expiresAt, CancellationToken cancellationToken = default)
        => _db.ExecuteAsync(
            @"UPDATE usuarios SET
                email_confirmation_token_hash = @hash,
                email_confirmation_expires_at = @expires
              WHERE id = @id",
            new Dictionary<string, object?>
            {
                ["@id"] = id,
                ["@hash"] = tokenHash,
                ["@expires"] = expiresAt
            }, cancellationToken);

    public Task ConfirmEmailAsync(Guid id, DateTime confirmedAt, CancellationToken cancellationToken = default)
        => _db.ExecuteAsync(
            @"UPDATE usuarios SET
                email_confirmed_at = @confirmedAt,
                email_confirmation_token_hash = NULL,
                email_confirmation_expires_at = NULL,
                status = @status,
                updated_at = @confirmedAt
              WHERE id = @id",
            new Dictionary<string, object?>
            {
                ["@id"] = id,
                ["@confirmedAt"] = confirmedAt,
                ["@status"] = (int)StatusAtivo.Ativo
            }, cancellationToken);

    public Task DeleteAsync(Guid id, Guid deletedBy, CancellationToken cancellationToken = default)
        => _db.ExecuteAsync(
            @"UPDATE usuarios SET deleted_at = @now, deleted_by = @by
              WHERE id = @id AND tenant_id = @tenant_id",
            new Dictionary<string, object?>
            {
                ["@id"] = id,
                ["@tenant_id"] = _tenantContext.TenantId,
                ["@now"] = DateTime.UtcNow,
                ["@by"] = deletedBy
            }, cancellationToken);

    private static Usuario Map(IDataRecord r) => new()
    {
        Id = r.GetValueOrDefault<Guid>("id"),
        TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
        NomeCompleto = r.GetValueOrDefault<string>("nome_completo") ?? string.Empty,
        Email = r.GetValueOrDefault<string>("email") ?? string.Empty,
        PasswordHash = r.GetValueOrDefault<string>("password_hash") ?? string.Empty,
        Status = (StatusAtivo)r.GetValueOrDefault<int>("status"),
        FailedLoginAttempts = r.GetValueOrDefault<int>("failed_login_attempts"),
        LockedUntil = r.GetValueOrDefault<DateTime?>("locked_until"),
        LastLoginAt = r.GetValueOrDefault<DateTime?>("last_login_at"),
        EmailConfirmedAt = r.GetValueOrDefault<DateTime?>("email_confirmed_at"),
        EmailConfirmationTokenHash = r.GetValueOrDefault<string>("email_confirmation_token_hash"),
        EmailConfirmationExpiresAt = r.GetValueOrDefault<DateTime?>("email_confirmation_expires_at"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
