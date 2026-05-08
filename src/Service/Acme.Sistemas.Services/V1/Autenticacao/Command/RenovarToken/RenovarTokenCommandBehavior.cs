using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Autenticacao.Command.RenovarToken;

/// <summary>
/// Behavior específico do RenovarTokenCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class RenovarTokenCommandBehavior
    : IPipelineBehavior<RenovarTokenCommand, ResponseDefault<RenovarTokenCommandResult>>
{
    public Task<ResponseDefault<RenovarTokenCommandResult>> Handle(
        RenovarTokenCommand request,
        RequestHandlerDelegate<ResponseDefault<RenovarTokenCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
