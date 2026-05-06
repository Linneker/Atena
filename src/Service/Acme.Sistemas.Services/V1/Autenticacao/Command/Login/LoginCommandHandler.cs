using Acme.Sistemas.Core.Erros;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Response.Erros;
using Acme.Sistemas.Core.Security;
using Acme.Sistemas.Domain.Entities.Permissions;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Autenticacao.Command.Login;

public sealed class LoginCommandHandler
    : IRequestHandler<LoginCommand, ResponseDefault<LoginCommandResult>>
{
    private const int MaxFailedAttempts = 5;
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15);

    private readonly IUsuarioRepository _users;
    private readonly IRolePermissionRepository _rolePermissions;
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly IJwtTokenService _jwt;

    public LoginCommandHandler(
        IUsuarioRepository users,
        IRolePermissionRepository rolePermissions,
        IRefreshTokenRepository refreshTokens,
        IJwtTokenService jwt)
    {
        _users = users;
        _rolePermissions = rolePermissions;
        _refreshTokens = refreshTokens;
        _jwt = jwt;
    }

    public async Task<ResponseDefault<LoginCommandResult>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _users.GetByEmailAcrossTenantsAsync(request.Email, cancellationToken);
        if (user is null || user.Status != StatusAtivo.Ativo)
        {
            return ResponseDefault<LoginCommandResult>.BadRequest(
                Error.Unauthorized(MessageErros.CredenciaisInvalidas));
        }

        if (user.IsLocked)
        {
            return ResponseDefault<LoginCommandResult>.BadRequest(
                Error.Unauthorized(MessageErros.ContaBloqueada));
        }

        if (!PasswordHelper.Verify(request.Senha, user.PasswordHash))
        {
            var attempts = user.FailedLoginAttempts + 1;
            DateTime? lockedUntil = attempts >= MaxFailedAttempts ? DateTime.UtcNow.Add(LockoutDuration) : null;
            await _users.UpdateLoginStatusAsync(user.Id, attempts, lockedUntil, user.LastLoginAt, cancellationToken);

            return ResponseDefault<LoginCommandResult>.BadRequest(
                Error.Unauthorized(lockedUntil.HasValue ? MessageErros.ContaBloqueada : MessageErros.CredenciaisInvalidas));
        }

        var now = DateTime.UtcNow;
        await _users.UpdateLoginStatusAsync(user.Id, 0, null, now, cancellationToken);

        var permissions = await _rolePermissions.GetCodigosByUserAsync(user.Id, cancellationToken);
        var tokens = _jwt.Issue(user.TenantId, user.Id, user.Email, permissions);

        var refreshHash = JwtTokenService.HashRefreshToken(tokens.RefreshToken);
        await _refreshTokens.AddAsync(new RefreshToken
        {
            Id = Guid.NewGuid(),
            TenantId = user.TenantId,
            UserId = user.Id,
            TokenHash = refreshHash,
            Jti = tokens.RefreshJti,
            IssuedAt = now,
            ExpiresAt = tokens.RefreshExpiresAt,
            UserAgent = request.UserAgent,
            IpAddress = request.IpAddress
        }, cancellationToken);

        return ResponseDefault<LoginCommandResult>.Ok(new LoginCommandResult(
            tokens.AccessToken,
            tokens.RefreshToken,
            tokens.AccessExpiresAt,
            tokens.RefreshExpiresAt,
            user.Id,
            user.TenantId,
            user.NomeCompleto,
            permissions));
    }
}
