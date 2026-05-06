namespace Acme.Sistemas.Core.Const;

public static class KeyCache
{
    public static string Tenant(Guid tenantId) => $"tenant:{tenantId}";
    public static string TenantBranding(Guid tenantId) => $"tenant:{tenantId}:branding";
    public static string TenantLimits(Guid tenantId) => $"tenant:{tenantId}:limits";
    public static string UserPermissions(Guid userId) => $"user:{userId}:permissions";
    public static string RolePermissions(Guid roleId) => $"role:{roleId}:permissions";
    public static string TokenBlacklist(string jti) => $"token:blacklist:{jti}";
    public static string LoginAttempts(string username) => $"login:attempts:{username}";
}
