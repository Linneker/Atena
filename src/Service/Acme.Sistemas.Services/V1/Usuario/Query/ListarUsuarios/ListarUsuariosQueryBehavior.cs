using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Usuario.Query.ListarUsuarios;

/// <summary>
/// Behavior específico do ListarUsuariosQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ListarUsuariosQueryBehavior
    : IPipelineBehavior<ListarUsuariosQuery, ResponseDefault<ListarUsuariosQueryResult>>
{
    public Task<ResponseDefault<ListarUsuariosQueryResult>> Handle(
        ListarUsuariosQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarUsuariosQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
