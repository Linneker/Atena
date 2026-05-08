using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.ContaReceber.Command.CriarContaReceber;

/// <summary>
/// Behavior específico do CriarContaReceberCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class CriarContaReceberCommandBehavior
    : IPipelineBehavior<CriarContaReceberCommand, ResponseDefault<CriarContaReceberCommandResult>>
{
    public Task<ResponseDefault<CriarContaReceberCommandResult>> Handle(
        CriarContaReceberCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarContaReceberCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
