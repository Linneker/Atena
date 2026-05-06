using System.Diagnostics;
using Acme.Sistemas.Domain.Entities.Auditoria;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Acme.Sistemas.Atena.Api.Middlewares;

/// <summary>
/// Registra cada requisição HTTP em <c>api_request_audit</c> de forma assíncrona
/// (sem bloquear a resposta). Pula health checks e a própria rota de auditoria.
/// </summary>
public sealed class ApiRequestAuditMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ApiRequestAuditMiddleware> _logger;

    public ApiRequestAuditMiddleware(
        RequestDelegate next,
        IServiceScopeFactory scopeFactory,
        ILogger<ApiRequestAuditMiddleware> logger)
    {
        _next = next;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value ?? string.Empty;
        if (DevePular(path))
        {
            await _next(context);
            return;
        }

        var sw = Stopwatch.StartNew();
        var correlationId = context.TraceIdentifier;
        Exception? captured = null;

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            captured = ex;
            throw;
        }
        finally
        {
            sw.Stop();
            var statusCode = captured is null ? context.Response.StatusCode : 500;

            // Snapshot dos campos antes de enfileirar (HttpContext pode ser reciclado)
            var audit = new ApiRequestAudit
            {
                Metodo = context.Request.Method,
                Caminho = path,
                QueryString = context.Request.QueryString.HasValue ? context.Request.QueryString.Value : null,
                StatusCode = statusCode,
                DuracaoMs = sw.ElapsedMilliseconds,
                IpAddress = context.Connection.RemoteIpAddress?.ToString(),
                UserAgent = context.Request.Headers.UserAgent.ToString(),
                CorrelationId = correlationId,
                OcorridoEm = DateTime.UtcNow
            };

            var tenantContext = context.RequestServices.GetService<ITenantContext>();
            audit.TenantId = tenantContext?.TenantId ?? Guid.Empty;
            audit.UserId = tenantContext?.UserId;

            // Persiste em background — não aguarda
            _ = PersistAsync(audit);
        }
    }

    private async Task PersistAsync(ApiRequestAudit audit)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IAuditLogRepository>();
            await repo.AddApiRequestAsync(audit, CancellationToken.None);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Falha ao persistir api_request_audit (non-fatal).");
        }
    }

    private static bool DevePular(string path) =>
        path.StartsWith("/health", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWith("/api/v1/auditoria", StringComparison.OrdinalIgnoreCase);
}

public static class ApiRequestAuditMiddlewareExtensions
{
    public static IApplicationBuilder UseApiRequestAudit(this IApplicationBuilder app)
        => app.UseMiddleware<ApiRequestAuditMiddleware>();
}
