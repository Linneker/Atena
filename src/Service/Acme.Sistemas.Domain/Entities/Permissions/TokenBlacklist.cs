namespace Acme.Sistemas.Domain.Entities.Permissions;

public sealed class TokenBlacklist
{
    public Guid Jti { get; set; }
    public Guid TenantId { get; set; }
    public Guid? UserId { get; set; }
    public DateTime BlacklistedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }
    public string? Reason { get; set; }
}
