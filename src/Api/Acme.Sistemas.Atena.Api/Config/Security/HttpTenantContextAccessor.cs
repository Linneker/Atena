using System.Security.Claims;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Atena.Api.Config.Security;

/// <summary>
/// Tenant context que combina HttpContext (uso normal por requisição)
/// com override manual (uso por workers/background services).
/// </summary>
public sealed class HttpTenantContextAccessor : IMutableTenantContext
{
    private readonly IHttpContextAccessor _accessor;
    private Guid? _overrideTenantId;
    private Guid? _overrideUserId;
    private IReadOnlySet<string>? _overridePermissions;
    private readonly Lazy<ContextSnapshot> _snapshot;

    public HttpTenantContextAccessor(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
        _snapshot = new Lazy<ContextSnapshot>(BuildSnapshot);
    }

    public Guid TenantId => _overrideTenantId ?? _snapshot.Value.TenantId;
    public Guid? UserId => _overrideUserId ?? _snapshot.Value.UserId;
    public bool IsAuthenticated => _overrideTenantId.HasValue || _snapshot.Value.IsAuthenticated;
    public IReadOnlySet<string> Permissions => _overridePermissions ?? _snapshot.Value.Permissions;

    /// <summary>
    /// Define o tenant manualmente — usado por background workers que não têm HttpContext.
    /// Deve ser chamado uma única vez por scope.
    /// </summary>
    public void Override(Guid tenantId, Guid? userId = null, IReadOnlySet<string>? permissions = null)
    {
        _overrideTenantId = tenantId;
        _overrideUserId = userId;
        _overridePermissions = permissions;
    }

    private ContextSnapshot BuildSnapshot()
    {
        var user = _accessor.HttpContext?.User;
        if (user?.Identity is null || !user.Identity.IsAuthenticated)
        {
            return new ContextSnapshot(Guid.Empty, null, false, new HashSet<string>());
        }

        var tenantClaim = user.FindFirst(TenantClaims.TenantId)?.Value;
        var userClaim = user.FindFirst(TenantClaims.UserId)?.Value
                        ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var tenantId = Guid.TryParse(tenantClaim, out var t) ? t : Guid.Empty;
        Guid? userId = Guid.TryParse(userClaim, out var u) ? u : null;

        var perms = user.FindAll(TenantClaims.Permissions)
            .Select(c => c.Value)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        return new ContextSnapshot(tenantId, userId, true, perms);
    }

    private sealed record ContextSnapshot(Guid TenantId, Guid? UserId, bool IsAuthenticated, IReadOnlySet<string> Permissions);
}
