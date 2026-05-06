namespace Acme.Sistemas.Domain.Entities.Permissions;

public sealed class ApiKey
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string KeyHash { get; set; } = string.Empty;
    public string? PermissionsJson { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid? CreatedBy { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public DateTime? LastUsedAt { get; set; }

    public bool IsActive =>
        RevokedAt is null && (ExpiresAt is null || ExpiresAt > DateTime.UtcNow);
}
