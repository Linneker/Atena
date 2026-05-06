using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Autenticacao.Command.Login;

public sealed record LoginCommand(
    string Email,
    string Senha,
    string? UserAgent,
    string? IpAddress) : IRequest<ResponseDefault<LoginCommandResult>>;

public sealed record LoginCommandResult(
    string AccessToken,
    string RefreshToken,
    DateTime AccessExpiresAt,
    DateTime RefreshExpiresAt,
    Guid UserId,
    Guid TenantId,
    string NomeCompleto,
    IReadOnlyList<string> Permissions);
