using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.DevolucaoVenda.Command.RegistrarDevolucao;

/// <summary>
/// Behavior específico do RegistrarDevolucaoCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class RegistrarDevolucaoCommandBehavior
    : IPipelineBehavior<RegistrarDevolucaoCommand, ResponseDefault<RegistrarDevolucaoCommandResult>>
{
    public Task<ResponseDefault<RegistrarDevolucaoCommandResult>> Handle(
        RegistrarDevolucaoCommand request,
        RequestHandlerDelegate<ResponseDefault<RegistrarDevolucaoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
