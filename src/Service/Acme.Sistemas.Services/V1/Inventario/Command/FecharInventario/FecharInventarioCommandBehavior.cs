using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Inventario.Command.FecharInventario;

/// <summary>
/// Behavior específico do FecharInventarioCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class FecharInventarioCommandBehavior
    : IPipelineBehavior<FecharInventarioCommand, ResponseDefault<FecharInventarioCommandResult>>
{
    public Task<ResponseDefault<FecharInventarioCommandResult>> Handle(
        FecharInventarioCommand request,
        RequestHandlerDelegate<ResponseDefault<FecharInventarioCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
