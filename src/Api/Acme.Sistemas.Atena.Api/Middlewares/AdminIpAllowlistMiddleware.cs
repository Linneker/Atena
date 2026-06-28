using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using Acme.Sistemas.Core.Settings;
using Microsoft.Extensions.Options;

namespace Acme.Sistemas.Atena.Api.Middlewares;

/// <summary>
/// Bloqueia rotas administrativas (<c>/api/v1/admin/*</c>) vindas de IPs fora da allowlist
/// configurada em <see cref="AdminOptions.AllowedIps"/> (CIDRs). Roda antes da autenticação:
/// um IP não-permitido recebe 403 sem sequer validar o token. Loopback é sempre permitido;
/// allowlist vazia = sem restrição de IP.
/// </summary>
public sealed class AdminIpAllowlistMiddleware
{
    private const string AdminPrefix = "/api/v1/admin";

    private readonly RequestDelegate _next;
    private readonly ILogger<AdminIpAllowlistMiddleware> _logger;
    private readonly IOptionsMonitor<AdminOptions> _options;

    public AdminIpAllowlistMiddleware(
        RequestDelegate next,
        ILogger<AdminIpAllowlistMiddleware> logger,
        IOptionsMonitor<AdminOptions> options)
    {
        _next = next;
        _logger = logger;
        _options = options;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value ?? string.Empty;
        if (!path.StartsWith(AdminPrefix, StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        var allowed = _options.CurrentValue.AllowedIps ?? Array.Empty<string>();
        var remoteIp = context.Connection.RemoteIpAddress;

        if (allowed.Length == 0 || IsLoopback(remoteIp) || IsInAnyCidr(remoteIp, allowed))
        {
            await _next(context);
            return;
        }

        _logger.LogWarning("Acesso admin bloqueado por allowlist de IP. IP={Ip} Path={Path}", remoteIp, path);
        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            status = (int)HttpStatusCode.Forbidden,
            message = "IP não autorizado para rotas administrativas."
        }));
    }

    private static bool IsLoopback(IPAddress? ip) => ip is not null && IPAddress.IsLoopback(ip);

    private static bool IsInAnyCidr(IPAddress? ip, IReadOnlyList<string> cidrs)
    {
        if (ip is null) return false;
        if (ip.IsIPv4MappedToIPv6) ip = ip.MapToIPv4();

        foreach (var cidr in cidrs)
        {
            if (TryMatchCidr(ip, cidr)) return true;
        }
        return false;
    }

    private static bool TryMatchCidr(IPAddress ip, string cidr)
    {
        var parts = cidr.Split('/', 2);
        if (!IPAddress.TryParse(parts[0], out var network)) return false;

        // Sem barra → comparação exata de endereço.
        if (parts.Length == 1)
            return ip.Equals(network);

        if (!int.TryParse(parts[1], out var prefixLength)) return false;
        if (ip.AddressFamily != network.AddressFamily) return false;

        var ipBytes = ip.GetAddressBytes();
        var netBytes = network.GetAddressBytes();
        if (ipBytes.Length != netBytes.Length) return false;

        var fullBytes = prefixLength / 8;
        var remainderBits = prefixLength % 8;

        for (var i = 0; i < fullBytes; i++)
        {
            if (ipBytes[i] != netBytes[i]) return false;
        }

        if (remainderBits == 0) return true;
        var mask = (byte)(0xFF << (8 - remainderBits));
        return (ipBytes[fullBytes] & mask) == (netBytes[fullBytes] & mask);
    }
}

public static class AdminIpAllowlistMiddlewareExtensions
{
    public static IApplicationBuilder UseAdminIpAllowlist(this IApplicationBuilder app)
        => app.UseMiddleware<AdminIpAllowlistMiddleware>();
}
