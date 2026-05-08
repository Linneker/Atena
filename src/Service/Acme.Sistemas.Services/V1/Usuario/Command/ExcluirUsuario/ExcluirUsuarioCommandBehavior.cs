using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Usuario.Command.ExcluirUsuario;

/// <summary>
/// Behavior específico do ExcluirUsuarioCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ExcluirUsuarioCommandBehavior
    : IPipelineBehavior<ExcluirUsuarioCommand, ResponseDefault>
{
    public Task<ResponseDefault> Handle(
        ExcluirUsuarioCommand request,
        RequestHandlerDelegate<ResponseDefault> next,
        CancellationToken cancellationToken) => next();
}
