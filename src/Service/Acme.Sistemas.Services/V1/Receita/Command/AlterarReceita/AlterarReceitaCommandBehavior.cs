using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Receita.Command.AlterarReceita;

/// <summary>
/// Behavior específico do AlterarReceitaCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class AlterarReceitaCommandBehavior
    : IPipelineBehavior<AlterarReceitaCommand, ResponseDefault<AlterarReceitaCommandResult>>
{
    public Task<ResponseDefault<AlterarReceitaCommandResult>> Handle(
        AlterarReceitaCommand request,
        RequestHandlerDelegate<ResponseDefault<AlterarReceitaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
