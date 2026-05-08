using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.PlanoDeContas.Command.CriarPlanoDeContas;

/// <summary>
/// Behavior específico do CriarPlanoDeContasCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class CriarPlanoDeContasCommandBehavior
    : IPipelineBehavior<CriarPlanoDeContasCommand, ResponseDefault<CriarPlanoDeContasCommandResult>>
{
    public Task<ResponseDefault<CriarPlanoDeContasCommandResult>> Handle(
        CriarPlanoDeContasCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarPlanoDeContasCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
