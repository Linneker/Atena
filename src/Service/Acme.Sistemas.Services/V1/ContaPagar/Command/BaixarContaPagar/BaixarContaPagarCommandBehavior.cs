using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.ContaPagar.Command.BaixarContaPagar;

/// <summary>
/// Behavior específico do BaixarContaPagarCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class BaixarContaPagarCommandBehavior
    : IPipelineBehavior<BaixarContaPagarCommand, ResponseDefault<BaixarContaPagarCommandResult>>
{
    public Task<ResponseDefault<BaixarContaPagarCommandResult>> Handle(
        BaixarContaPagarCommand request,
        RequestHandlerDelegate<ResponseDefault<BaixarContaPagarCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
