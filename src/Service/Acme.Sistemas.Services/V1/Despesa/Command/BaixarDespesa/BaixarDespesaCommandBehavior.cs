using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Despesa.Command.BaixarDespesa;

/// <summary>
/// Behavior específico do BaixarDespesaCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class BaixarDespesaCommandBehavior
    : IPipelineBehavior<BaixarDespesaCommand, ResponseDefault<BaixarDespesaCommandResult>>
{
    public Task<ResponseDefault<BaixarDespesaCommandResult>> Handle(
        BaixarDespesaCommand request,
        RequestHandlerDelegate<ResponseDefault<BaixarDespesaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
