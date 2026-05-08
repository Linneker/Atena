using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Fiscal.Command.EmitirNFe;

/// <summary>
/// Behavior específico do EmitirNFeCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class EmitirNFeCommandBehavior
    : IPipelineBehavior<EmitirNFeCommand, ResponseDefault<EmitirNFeCommandResult>>
{
    public Task<ResponseDefault<EmitirNFeCommandResult>> Handle(
        EmitirNFeCommand request,
        RequestHandlerDelegate<ResponseDefault<EmitirNFeCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
