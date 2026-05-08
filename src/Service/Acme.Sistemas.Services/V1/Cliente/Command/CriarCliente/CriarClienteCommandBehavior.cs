using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Cliente.Command.CriarCliente;

/// <summary>
/// Behavior específico do CriarClienteCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class CriarClienteCommandBehavior
    : IPipelineBehavior<CriarClienteCommand, ResponseDefault<CriarClienteCommandResult>>
{
    public Task<ResponseDefault<CriarClienteCommandResult>> Handle(
        CriarClienteCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarClienteCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
