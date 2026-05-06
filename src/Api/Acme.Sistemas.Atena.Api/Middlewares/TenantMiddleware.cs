using System.Net;
using System.Text.Json;
using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Core.Erros;

namespace Acme.Sistemas.Atena.Api.Middlewares;

public sealed class TenantMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TenantMiddleware> _logger;

    private static readonly string[] AnonymousPaths =
    {
        "/health",
        "/swagger",
        "/api/v1/auth/login",
        "/api/v1/auth/refresh",
        "/api/v1/tenants/registrar"
    };

    public TenantMiddleware(RequestDelegate next, ILogger<TenantMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value ?? string.Empty;
        if (IsAnonymous(path))
        {
            await _next(context);
            return;
        }

        var user = context.User;
        if (user.Identity?.IsAuthenticated != true)
        {
            await WriteProblemAsync(context, HttpStatusCode.Unauthorized, MessageErros.TokenInvalido);
            return;
        }

        var tenantClaim = user.FindFirst(TenantClaims.TenantId)?.Value;
        if (!Guid.TryParse(tenantClaim, out var tenantId) || tenantId == Guid.Empty)
        {
            _logger.LogWarning("Requisição autenticada sem tenant_id válido. Path={Path}", path);
            await WriteProblemAsync(context, HttpStatusCode.Unauthorized, MessageErros.TenantInvalido);
            return;
        }

        context.Items["TenantId"] = tenantId;
        await _next(context);
    }

    private static bool IsAnonymous(string path)
        => AnonymousPaths.Any(prefix => path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));

    private static async Task WriteProblemAsync(HttpContext context, HttpStatusCode status, string message)
    {
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/json";
        var payload = JsonSerializer.Serialize(new { status = (int)status, message });
        await context.Response.WriteAsync(payload);
    }
}

public static class TenantMiddlewareExtensions
{
    public static IApplicationBuilder UseTenantContext(this IApplicationBuilder app)
        => app.UseMiddleware<TenantMiddleware>();
}
