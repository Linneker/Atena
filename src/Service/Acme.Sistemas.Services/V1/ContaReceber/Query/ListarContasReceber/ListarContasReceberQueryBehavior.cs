using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.ContaReceber.Query.ListarContasReceber;

/// <summary>
/// Behavior específico do ListarContasReceberQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ListarContasReceberQueryBehavior
    : IPipelineBehavior<ListarContasReceberQuery, ResponseDefault<ListarContasReceberQueryResult>>
{
    public Task<ResponseDefault<ListarContasReceberQueryResult>> Handle(
        ListarContasReceberQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarContasReceberQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
