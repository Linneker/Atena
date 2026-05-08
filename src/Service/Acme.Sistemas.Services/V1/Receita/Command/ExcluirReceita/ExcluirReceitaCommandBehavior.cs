using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Receita.Command.ExcluirReceita;

/// <summary>
/// Behavior específico do ExcluirReceitaCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ExcluirReceitaCommandBehavior
    : IPipelineBehavior<ExcluirReceitaCommand, ResponseDefault>
{
    public Task<ResponseDefault> Handle(
        ExcluirReceitaCommand request,
        RequestHandlerDelegate<ResponseDefault> next,
        CancellationToken cancellationToken) => next();
}
