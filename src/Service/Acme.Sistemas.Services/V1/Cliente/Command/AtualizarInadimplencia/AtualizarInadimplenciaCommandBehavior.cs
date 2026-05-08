using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Cliente.Command.AtualizarInadimplencia;

/// <summary>
/// Behavior específico do AtualizarInadimplenciaCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class AtualizarInadimplenciaCommandBehavior
    : IPipelineBehavior<AtualizarInadimplenciaCommand, ResponseDefault<AtualizarInadimplenciaCommandResult>>
{
    public Task<ResponseDefault<AtualizarInadimplenciaCommandResult>> Handle(
        AtualizarInadimplenciaCommand request,
        RequestHandlerDelegate<ResponseDefault<AtualizarInadimplenciaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
