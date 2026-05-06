using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Acme.Sistemas.Atena.Api.Config.Security;

public sealed class PermissaoRequirement : IAuthorizationRequirement
{
    public string Codigo { get; }
    public PermissaoRequirement(string codigo) { Codigo = codigo; }
}

public sealed class PermissaoAuthorizationHandler : AuthorizationHandler<PermissaoRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissaoRequirement requirement)
    {
        if (context.User.Identity?.IsAuthenticated != true) return Task.CompletedTask;

        var hasPermission = context.User.FindAll(TenantClaims.Permissions)
            .Any(c => string.Equals(c.Value, requirement.Codigo, StringComparison.OrdinalIgnoreCase));

        if (hasPermission) context.Succeed(requirement);
        return Task.CompletedTask;
    }
}

public static class PermissaoAuthorizationExtensions
{
    public static TBuilder RequirePermissao<TBuilder>(this TBuilder builder, string codigo)
        where TBuilder : IEndpointConventionBuilder
    {
        builder.RequireAuthorization(new AuthorizeAttribute
        {
            Policy = $"perm:{codigo}"
        });
        return builder;
    }

    public static AuthorizationOptions AddPermissionPolicies(this AuthorizationOptions options, IEnumerable<string> codigos)
    {
        foreach (var codigo in codigos)
        {
            var policyName = $"perm:{codigo}";
            options.AddPolicy(policyName, p => p.Requirements.Add(new PermissaoRequirement(codigo)));
        }
        return options;
    }
}
