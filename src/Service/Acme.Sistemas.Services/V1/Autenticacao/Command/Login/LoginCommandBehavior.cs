using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Autenticacao.Command.Login;

/// <summary>
/// Behavior específico do LoginCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class LoginCommandBehavior
    : IPipelineBehavior<LoginCommand, ResponseDefault<LoginCommandResult>>
{
    public Task<ResponseDefault<LoginCommandResult>> Handle(
        LoginCommand request,
        RequestHandlerDelegate<ResponseDefault<LoginCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
