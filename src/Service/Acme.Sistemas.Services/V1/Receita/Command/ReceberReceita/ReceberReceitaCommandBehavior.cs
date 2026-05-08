using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Receita.Command.ReceberReceita;

/// <summary>
/// Behavior específico do ReceberReceitaCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ReceberReceitaCommandBehavior
    : IPipelineBehavior<ReceberReceitaCommand, ResponseDefault<ReceberReceitaCommandResult>>
{
    public Task<ResponseDefault<ReceberReceitaCommandResult>> Handle(
        ReceberReceitaCommand request,
        RequestHandlerDelegate<ResponseDefault<ReceberReceitaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
