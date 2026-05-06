using System.Security.Claims;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Atena.Api.Config.Security;

public sealed class HttpTenantContextAccessor : ITenantContext
{
    private readonly IHttpContextAccessor _accessor;
    private readonly Lazy<ContextSnapshot> _snapshot;

    public HttpTenantContextAccessor(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
        _snapshot = new Lazy<ContextSnapshot>(BuildSnapshot);
    }

    public Guid TenantId => _snapshot.Value.TenantId;
    public Guid? UserId => _snapshot.Value.UserId;
    public bool IsAuthenticated => _snapshot.Value.IsAuthenticated;
    public IReadOnlySet<string> Permissions => _snapshot.Value.Permissions;

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
