using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Cliente.Command.AlterarCliente;

/// <summary>
/// Behavior específico do AlterarClienteCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class AlterarClienteCommandBehavior
    : IPipelineBehavior<AlterarClienteCommand, ResponseDefault<AlterarClienteCommandResult>>
{
    public Task<ResponseDefault<AlterarClienteCommandResult>> Handle(
        AlterarClienteCommand request,
        RequestHandlerDelegate<ResponseDefault<AlterarClienteCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
