namespace Acme.Sistemas.Domain.Interfaces.Repository;

public interface ITenantContext
{
    Guid TenantId { get; }
    Guid? UserId { get; }
    bool IsAuthenticated { get; }
    IReadOnlySet<string> Permissions { get; }
}
