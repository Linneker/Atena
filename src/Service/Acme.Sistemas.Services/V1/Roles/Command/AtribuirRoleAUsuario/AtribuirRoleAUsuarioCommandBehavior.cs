using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Roles.Command.AtribuirRoleAUsuario;

/// <summary>
/// Behavior específico do AtribuirRoleAUsuarioCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class AtribuirRoleAUsuarioCommandBehavior
    : IPipelineBehavior<AtribuirRoleAUsuarioCommand, ResponseDefault>
{
    public Task<ResponseDefault> Handle(
        AtribuirRoleAUsuarioCommand request,
        RequestHandlerDelegate<ResponseDefault> next,
        CancellationToken cancellationToken) => next();
}
