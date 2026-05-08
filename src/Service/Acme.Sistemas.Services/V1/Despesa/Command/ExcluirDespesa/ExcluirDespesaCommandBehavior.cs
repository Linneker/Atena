using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Despesa.Command.ExcluirDespesa;

/// <summary>
/// Behavior específico do ExcluirDespesaCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ExcluirDespesaCommandBehavior
    : IPipelineBehavior<ExcluirDespesaCommand, ResponseDefault>
{
    public Task<ResponseDefault> Handle(
        ExcluirDespesaCommand request,
        RequestHandlerDelegate<ResponseDefault> next,
        CancellationToken cancellationToken) => next();
}
