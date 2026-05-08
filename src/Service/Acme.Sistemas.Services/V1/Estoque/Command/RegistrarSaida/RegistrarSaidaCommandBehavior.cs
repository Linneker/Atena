using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Estoque.Command.RegistrarSaida;

/// <summary>
/// Behavior específico do RegistrarSaidaCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class RegistrarSaidaCommandBehavior
    : IPipelineBehavior<RegistrarSaidaCommand, ResponseDefault<RegistrarSaidaCommandResult>>
{
    public Task<ResponseDefault<RegistrarSaidaCommandResult>> Handle(
        RegistrarSaidaCommand request,
        RequestHandlerDelegate<ResponseDefault<RegistrarSaidaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
