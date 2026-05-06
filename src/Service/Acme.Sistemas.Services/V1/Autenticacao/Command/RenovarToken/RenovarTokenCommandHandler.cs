using Acme.Sistemas.Core.Erros;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Response.Erros;
using Acme.Sistemas.Core.Security;
using Acme.Sistemas.Domain.Entities.Permissions;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Autenticacao.Command.RenovarToken;

public sealed class RenovarTokenCommandHandler
    : IRequestHandler<RenovarTokenCommand, ResponseDefault<RenovarTokenCommandResult>>
{
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly IUsuarioRepository _users;
    private readonly IRolePermissionRepository _rolePermissions;
    private readonly IJwtTokenService _jwt;

    public RenovarTokenCommandHandler(
        IRefreshTokenRepository refreshTokens,
        IUsuarioRepository users,
        IRolePermissionRepository rolePermissions,
        IJwtTokenService jwt)
    {
        _refreshTokens = refreshTokens;
        _users = users;
        _rolePermissions = rolePermissions;
        _jwt = jwt;
    }

    public async Task<ResponseDefault<RenovarTokenCommandResult>> Handle(
        RenovarTokenCommand request,
        CancellationToken cancellationToken)
    {
        var hash = JwtTokenService.HashRefreshToken(request.RefreshToken);
        var existing = await _refreshTokens.GetByHashAsync(hash, cancellationToken);

        if (existing is null || !existing.IsActive)
        {
            return ResponseDefault<RenovarTokenCommandResult>.BadRequest(
                Error.Unauthorized(MessageErros.TokenInvalido));
        }

        var dbUser = await _users.GetByIdAcrossTenantsAsync(existing.UserId, cancellationToken);
        if (dbUser is null)
        {
            return ResponseDefault<RenovarTokenCommandResult>.BadRequest(
                Error.Unauthorized(MessageErros.TokenInvalido));
        }

        var permissions = await _rolePermissions.GetCodigosByUserAsync(dbUser.Id, cancellationToken);
        var newTokens = _jwt.Issue(dbUser.TenantId, dbUser.Id, dbUser.Email, permissions);
        var newHash = JwtTokenService.HashRefreshToken(newTokens.RefreshToken);
        var now = DateTime.UtcNow;

        var newRecord = new RefreshToken
        {
            Id = Guid.NewGuid(),
            TenantId = dbUser.TenantId,
            UserId = dbUser.Id,
            TokenHash = newHash,
            Jti = newTokens.RefreshJti,
            IssuedAt = now,
            ExpiresAt = newTokens.RefreshExpiresAt,
            UserAgent = request.UserAgent,
            IpAddress = request.IpAddress
        };

        await _refreshTokens.AddAsync(newRecord, cancellationToken);
        await _refreshTokens.RevokeAsync(existing.Id, newRecord.Id, cancellationToken);

        return ResponseDefault<RenovarTokenCommandResult>.Ok(new RenovarTokenCommandResult(
            newTokens.AccessToken,
            newTokens.RefreshToken,
            newTokens.AccessExpiresAt,
            newTokens.RefreshExpiresAt));
    }
}
