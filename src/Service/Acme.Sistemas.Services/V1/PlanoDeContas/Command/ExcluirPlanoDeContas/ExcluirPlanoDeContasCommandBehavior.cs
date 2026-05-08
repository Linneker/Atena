using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.PlanoDeContas.Command.ExcluirPlanoDeContas;

/// <summary>
/// Behavior específico do ExcluirPlanoDeContasCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ExcluirPlanoDeContasCommandBehavior
    : IPipelineBehavior<ExcluirPlanoDeContasCommand, ResponseDefault>
{
    public Task<ResponseDefault> Handle(
        ExcluirPlanoDeContasCommand request,
        RequestHandlerDelegate<ResponseDefault> next,
        CancellationToken cancellationToken) => next();
}
