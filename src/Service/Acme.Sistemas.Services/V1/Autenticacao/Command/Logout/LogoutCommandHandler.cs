using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Security;
using Acme.Sistemas.Domain.Entities.Permissions;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Autenticacao.Command.Logout;

public sealed class LogoutCommandHandler : IRequestHandler<LogoutCommand, ResponseDefault>
{
    private readonly IRefreshTokenRepository _refreshTokens;
    private readonly ITokenBlacklistRepository _blacklist;

    public LogoutCommandHandler(
        IRefreshTokenRepository refreshTokens,
        ITokenBlacklistRepository blacklist)
    {
        _refreshTokens = refreshTokens;
        _blacklist = blacklist;
    }

    public async Task<ResponseDefault> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var hash = JwtTokenService.HashRefreshToken(request.RefreshToken);
        var token = await _refreshTokens.GetByHashAsync(hash, cancellationToken);

        if (token is not null && token.IsActive)
        {
            await _refreshTokens.RevokeAsync(token.Id, replacedBy: null, cancellationToken);

            await _blacklist.AddAsync(new TokenBlacklist
            {
                Jti = token.Jti,
                TenantId = token.TenantId,
                UserId = token.UserId,
                BlacklistedAt = DateTime.UtcNow,
                ExpiresAt = token.ExpiresAt,
                Reason = "Logout"
            }, cancellationToken);
        }

        return ResponseDefault.NoContent();
    }
}
