using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Despesa.Command.AlterarDespesa;

/// <summary>
/// Behavior específico do AlterarDespesaCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class AlterarDespesaCommandBehavior
    : IPipelineBehavior<AlterarDespesaCommand, ResponseDefault<AlterarDespesaCommandResult>>
{
    public Task<ResponseDefault<AlterarDespesaCommandResult>> Handle(
        AlterarDespesaCommand request,
        RequestHandlerDelegate<ResponseDefault<AlterarDespesaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
