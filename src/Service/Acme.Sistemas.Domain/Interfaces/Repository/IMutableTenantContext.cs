namespace Acme.Sistemas.Domain.Interfaces.Repository;

/// <summary>
/// Permite que workers/background services definam o tenant manualmente
/// (fora do contexto HTTP).
/// </summary>
public interface IMutableTenantContext : ITenantContext
{
    void Override(Guid tenantId, Guid? userId = null, IReadOnlySet<string>? permissions = null);
}
