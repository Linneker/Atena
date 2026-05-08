using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.ContaReceber.Command.ReceberContaReceber;

/// <summary>
/// Behavior específico do ReceberContaReceberCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ReceberContaReceberCommandBehavior
    : IPipelineBehavior<ReceberContaReceberCommand, ResponseDefault<ReceberContaReceberCommandResult>>
{
    public Task<ResponseDefault<ReceberContaReceberCommandResult>> Handle(
        ReceberContaReceberCommand request,
        RequestHandlerDelegate<ResponseDefault<ReceberContaReceberCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
