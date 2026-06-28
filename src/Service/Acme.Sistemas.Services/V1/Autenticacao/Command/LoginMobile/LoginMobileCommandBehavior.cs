using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Autenticacao.Command.LoginMobile;

public sealed class LoginMobileCommandBehavior
    : IPipelineBehavior<LoginMobileCommand, ResponseDefault<LoginMobileCommandResult>>
{
    public Task<ResponseDefault<LoginMobileCommandResult>> Handle(
        LoginMobileCommand request,
        RequestHandlerDelegate<ResponseDefault<LoginMobileCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
