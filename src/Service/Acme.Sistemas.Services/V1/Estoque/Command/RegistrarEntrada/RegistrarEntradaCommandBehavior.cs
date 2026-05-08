using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Estoque.Command.RegistrarEntrada;

/// <summary>
/// Behavior específico do RegistrarEntradaCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class RegistrarEntradaCommandBehavior
    : IPipelineBehavior<RegistrarEntradaCommand, ResponseDefault<RegistrarEntradaCommandResult>>
{
    public Task<ResponseDefault<RegistrarEntradaCommandResult>> Handle(
        RegistrarEntradaCommand request,
        RequestHandlerDelegate<ResponseDefault<RegistrarEntradaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
