namespace Acme.Sistemas.Domain.Entities.Permissions;

public sealed class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TenantId { get; set; }
    public Guid UserId { get; set; }
    public string TokenHash { get; set; } = string.Empty;
    public Guid Jti { get; set; }
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public Guid? ReplacedBy { get; set; }
    public string? UserAgent { get; set; }
    public string? IpAddress { get; set; }

    public bool IsActive => RevokedAt is null && ExpiresAt > DateTime.UtcNow;
}
