using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Cliente.Query.ListarClientes;

/// <summary>
/// Behavior específico do ListarClientesQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ListarClientesQueryBehavior
    : IPipelineBehavior<ListarClientesQuery, ResponseDefault<ListarClientesQueryResult>>
{
    public Task<ResponseDefault<ListarClientesQueryResult>> Handle(
        ListarClientesQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarClientesQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
