using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Acme.Sistemas.Core.Security;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly JwtOptions _options;
    private readonly SigningCredentials _signingCredentials;
    private readonly TokenValidationParameters _validationParameters;

    public JwtTokenService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
        var keyBytes = Encoding.UTF8.GetBytes(_options.SigningKey);
        var key = new SymmetricSecurityKey(keyBytes);
        _signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        _validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = _options.Issuer,
            ValidateAudience = true,
            ValidAudience = _options.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    }

    public JwtTokenPair Issue(Guid tenantId, Guid userId, string email, IReadOnlyCollection<string> permissions)
    {
        var now = DateTime.UtcNow;
        var accessJti = Guid.NewGuid();
        var refreshJti = Guid.NewGuid();
        var accessExpires = now.AddMinutes(_options.AccessTokenMinutes);
        var refreshExpires = now.AddDays(_options.RefreshTokenDays);

        var claims = new List<Claim>
        {
            new("tenant_id", tenantId.ToString()),
            new("sub", userId.ToString()),
            new("email", email),
            new("jti", accessJti.ToString())
        };
        foreach (var p in permissions) claims.Add(new Claim("perm", p));

        var jwt = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: now,
            expires: accessExpires,
            signingCredentials: _signingCredentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
        var refreshToken = GenerateRefreshTokenString();

        return new JwtTokenPair(accessToken, refreshToken, accessJti, refreshJti, accessExpires, refreshExpires);
    }

    public bool TryValidate(string accessToken, out Guid jti, out Guid tenantId, out Guid userId)
    {
        jti = Guid.Empty;
        tenantId = Guid.Empty;
        userId = Guid.Empty;
        try
        {
            var handler = new JwtSecurityTokenHandler { MapInboundClaims = false };
            var principal = handler.ValidateToken(accessToken, _validationParameters, out _);
            Guid.TryParse(principal.FindFirst("jti")?.Value, out jti);
            Guid.TryParse(principal.FindFirst("tenant_id")?.Value, out tenantId);
            Guid.TryParse(principal.FindFirst("sub")?.Value, out userId);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static string HashRefreshToken(string token) => Hash.Sha512(token);

    private static string GenerateRefreshTokenString()
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }
}
