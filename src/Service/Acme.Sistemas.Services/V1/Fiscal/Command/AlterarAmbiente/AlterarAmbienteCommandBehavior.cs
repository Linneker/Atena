using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Fiscal.Command.AlterarAmbiente;

/// <summary>
/// Behavior específico do AlterarAmbienteCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class AlterarAmbienteCommandBehavior
    : IPipelineBehavior<AlterarAmbienteCommand, ResponseDefault<AlterarAmbienteCommandResult>>
{
    public Task<ResponseDefault<AlterarAmbienteCommandResult>> Handle(
        AlterarAmbienteCommand request,
        RequestHandlerDelegate<ResponseDefault<AlterarAmbienteCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
