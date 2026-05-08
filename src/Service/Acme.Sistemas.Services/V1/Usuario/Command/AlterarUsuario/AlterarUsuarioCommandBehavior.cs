using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Usuario.Command.AlterarUsuario;

/// <summary>
/// Behavior específico do AlterarUsuarioCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class AlterarUsuarioCommandBehavior
    : IPipelineBehavior<AlterarUsuarioCommand, ResponseDefault<AlterarUsuarioCommandResult>>
{
    public Task<ResponseDefault<AlterarUsuarioCommandResult>> Handle(
        AlterarUsuarioCommand request,
        RequestHandlerDelegate<ResponseDefault<AlterarUsuarioCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
