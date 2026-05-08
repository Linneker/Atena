using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Autenticacao.Command.Logout;

/// <summary>
/// Behavior específico do LogoutCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class LogoutCommandBehavior
    : IPipelineBehavior<LogoutCommand, ResponseDefault>
{
    public Task<ResponseDefault> Handle(
        LogoutCommand request,
        RequestHandlerDelegate<ResponseDefault> next,
        CancellationToken cancellationToken) => next();
}
