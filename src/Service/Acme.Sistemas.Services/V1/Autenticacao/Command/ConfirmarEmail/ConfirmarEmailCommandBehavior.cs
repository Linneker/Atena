using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Autenticacao.Command.ConfirmarEmail;

/// <summary>
/// Behavior específico do ConfirmarEmailCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ConfirmarEmailCommandBehavior
    : IPipelineBehavior<ConfirmarEmailCommand, ResponseDefault<ConfirmarEmailCommandResult>>
{
    public Task<ResponseDefault<ConfirmarEmailCommandResult>> Handle(
        ConfirmarEmailCommand request,
        RequestHandlerDelegate<ResponseDefault<ConfirmarEmailCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
