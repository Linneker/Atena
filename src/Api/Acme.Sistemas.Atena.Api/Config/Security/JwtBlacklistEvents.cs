using Microsoft.AspNetCore.Authentication.JwtBearer;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Atena.Api.Config.Security;

public static class JwtBlacklistEvents
{
    public static JwtBearerEvents Build() => new()
    {
        OnTokenValidated = async context =>
        {
            var jtiClaim = context.Principal?.FindFirst("jti")?.Value;
            if (!Guid.TryParse(jtiClaim, out var jti)) return;

            var blacklist = context.HttpContext.RequestServices
                .GetRequiredService<ITokenBlacklistRepository>();

            if (await blacklist.IsBlacklistedAsync(jti, context.HttpContext.RequestAborted))
            {
                context.Fail("Token revogado.");
            }
        }
    };
}
