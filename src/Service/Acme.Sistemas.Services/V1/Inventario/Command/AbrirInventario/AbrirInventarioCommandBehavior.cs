using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Inventario.Command.AbrirInventario;

/// <summary>
/// Behavior específico do AbrirInventarioCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class AbrirInventarioCommandBehavior
    : IPipelineBehavior<AbrirInventarioCommand, ResponseDefault<AbrirInventarioCommandResult>>
{
    public Task<ResponseDefault<AbrirInventarioCommandResult>> Handle(
        AbrirInventarioCommand request,
        RequestHandlerDelegate<ResponseDefault<AbrirInventarioCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
