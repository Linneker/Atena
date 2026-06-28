using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Autenticacao.Command.LoginMobile;

public sealed record LoginMobileCommand(
    string Email,
    string Senha,
    string DeviceId,
    string Plataforma,
    string? IpAddress,
    string? UserAgent) : IRequest<ResponseDefault<LoginMobileCommandResult>>;
