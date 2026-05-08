using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Roles.Command.CriarRole;

/// <summary>
/// Behavior específico do CriarRoleCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class CriarRoleCommandBehavior
    : IPipelineBehavior<CriarRoleCommand, ResponseDefault<CriarRoleCommandResult>>
{
    public Task<ResponseDefault<CriarRoleCommandResult>> Handle(
        CriarRoleCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarRoleCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
