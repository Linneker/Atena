namespace Acme.Sistemas.Atena.Api.Config.Security;

public static class TenantClaims
{
    public const string TenantId = "tenant_id";
    public const string UserId = "sub";
    public const string Permissions = "perm";
    public const string Roles = "role";
}
