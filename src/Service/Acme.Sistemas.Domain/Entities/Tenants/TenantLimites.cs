namespace Acme.Sistemas.Domain.Entities.Tenants;

public sealed class TenantLimites
{
    public Guid TenantId { get; set; }
    public int MaxUsuarios { get; set; }
    public int MaxNFeMes { get; set; }
    public int MaxStorageGb { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
