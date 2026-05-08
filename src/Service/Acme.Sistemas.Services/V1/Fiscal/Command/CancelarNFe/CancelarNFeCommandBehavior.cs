using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Fiscal.Command.CancelarNFe;

/// <summary>
/// Behavior específico do CancelarNFeCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class CancelarNFeCommandBehavior
    : IPipelineBehavior<CancelarNFeCommand, ResponseDefault<CancelarNFeCommandResult>>
{
    public Task<ResponseDefault<CancelarNFeCommandResult>> Handle(
        CancelarNFeCommand request,
        RequestHandlerDelegate<ResponseDefault<CancelarNFeCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
