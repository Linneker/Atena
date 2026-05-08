using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.PlanoDeContas.Command.AlterarPlanoDeContas;

/// <summary>
/// Behavior específico do AlterarPlanoDeContasCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class AlterarPlanoDeContasCommandBehavior
    : IPipelineBehavior<AlterarPlanoDeContasCommand, ResponseDefault<AlterarPlanoDeContasCommandResult>>
{
    public Task<ResponseDefault<AlterarPlanoDeContasCommandResult>> Handle(
        AlterarPlanoDeContasCommand request,
        RequestHandlerDelegate<ResponseDefault<AlterarPlanoDeContasCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
