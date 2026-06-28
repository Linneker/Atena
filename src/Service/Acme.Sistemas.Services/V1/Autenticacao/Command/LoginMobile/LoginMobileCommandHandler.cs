using Acme.Sistemas.Core.Erros;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Response.Erros;
using Acme.Sistemas.Core.Security;
using Acme.Sistemas.Domain.Entities.Permissions;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Autenticacao.Command.LoginMobile;

/// <summary>
/// Variante mobile do Login. Diferenças do Login web:
/// 1. Usa IssueMobile (refresh token de 90 dias, não 7)
/// 2. Carrega deviceId no userAgent (auditoria)
/// 3. Em handler de bater ponto, deviceId valida que device está registrado para o usuário
/// </summary>
public sealed class LoginMobileCommandHandler
    : IRequestHandler<LoginMobileCommand, ResponseDefault<LoginMobileCommandResult>>
{
    private const int MaxFailedAttempts = 5;
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(15);

    private readonly IUsuarioRepository _users;
    private readonly ITenantRepository _tenants;
    private readonly IRolePermissionRepository _rolePermissions;
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly IJwtTokenService _jwt;

    public LoginMobileCommandHandler(
        IUsuarioRepository users,
        ITenantRepository tenants,
        IRolePermissionRepository rolePermissions,
        IRefreshTokenRepository refreshTokens,
        IJwtTokenService jwt)
    {
        _users = users;
        _tenants = tenants;
        _rolePermissions = rolePermissions;
        _refreshTokens = refreshTokens;
        _jwt = jwt;
    }

    public async Task<ResponseDefault<LoginMobileCommandResult>> Handle(
        LoginMobileCommand request, CancellationToken cancellationToken)
    {
        var user = await _users.GetByEmailAcrossTenantsAsync(request.Email, cancellationToken);
        if (user is null)
            return ResponseDefault<LoginMobileCommandResult>.BadRequest(
                Error.Unauthorized(MessageErros.CredenciaisInvalidas));

        var tenant = await _tenants.GetByIdAsync(user.TenantId, cancellationToken);
        if (tenant is null || tenant.Status != StatusAtivo.Ativo)
            return ResponseDefault<LoginMobileCommandResult>.BadRequest(
                Error.Unauthorized(MessageErros.CredenciaisInvalidas));

        if (user.Status == StatusAtivo.PendenteConfirmacao || !user.IsEmailConfirmed)
            return ResponseDefault<LoginMobileCommandResult>.BadRequest(
                Error.Unauthorized(MessageErros.EmailNaoConfirmado));

        if (user.Status != StatusAtivo.Ativo)
            return ResponseDefault<LoginMobileCommandResult>.BadRequest(
                Error.Unauthorized(MessageErros.CredenciaisInvalidas));

        if (user.IsLocked)
            return ResponseDefault<LoginMobileCommandResult>.BadRequest(
                Error.Unauthorized(MessageErros.ContaBloqueada));

        if (!PasswordHelper.Verify(request.Senha, user.PasswordHash))
        {
            var attempts = user.FailedLoginAttempts + 1;
            DateTime? lockedUntil = attempts >= MaxFailedAttempts ? DateTime.UtcNow.Add(LockoutDuration) : null;
            await _users.UpdateLoginStatusAsync(user.Id, attempts, lockedUntil, user.LastLoginAt, cancellationToken);
            return ResponseDefault<LoginMobileCommandResult>.BadRequest(
                Error.Unauthorized(lockedUntil.HasValue ? MessageErros.ContaBloqueada : MessageErros.CredenciaisInvalidas));
        }

        var now = DateTime.UtcNow;
        await _users.UpdateLoginStatusAsync(user.Id, 0, null, now, cancellationToken);

        var permissions = await _rolePermissions.GetCodigosByUserAsync(user.Id, cancellationToken);
        // ⚠ IssueMobile gera refresh de 90 dias (em vez de 7) para reduzir re-login no app
        var tokens = _jwt.IssueMobile(user.TenantId, user.Id, user.Email, user.NomeCompleto, permissions);

        var refreshHash = JwtTokenService.HashRefreshToken(tokens.RefreshToken);
        var userAgentEnriquecido = $"mobile/{request.Plataforma}/{request.DeviceId} {request.UserAgent}".Trim();
        await _refreshTokens.AddAsync(new RefreshToken
        {
            Id = Guid.NewGuid(),
            TenantId = user.TenantId,
            UserId = user.Id,
            TokenHash = refreshHash,
            Jti = tokens.RefreshJti,
            IssuedAt = now,
            ExpiresAt = tokens.RefreshExpiresAt,
            UserAgent = userAgentEnriquecido,
            IpAddress = request.IpAddress
        }, cancellationToken);

        return ResponseDefault<LoginMobileCommandResult>.Ok(new LoginMobileCommandResult(
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
