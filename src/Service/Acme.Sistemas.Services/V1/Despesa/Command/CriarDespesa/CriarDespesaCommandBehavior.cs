using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Despesa.Command.CriarDespesa;

/// <summary>
/// Behavior específico do CriarDespesaCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class CriarDespesaCommandBehavior
    : IPipelineBehavior<CriarDespesaCommand, ResponseDefault<CriarDespesaCommandResult>>
{
    public Task<ResponseDefault<CriarDespesaCommandResult>> Handle(
        CriarDespesaCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarDespesaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
