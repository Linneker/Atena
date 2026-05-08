using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Cliente.Query.ObterCliente;

/// <summary>
/// Behavior específico do ObterClienteQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ObterClienteQueryBehavior
    : IPipelineBehavior<ObterClienteQuery, ResponseDefault<ObterClienteQueryResult>>
{
    public Task<ResponseDefault<ObterClienteQueryResult>> Handle(
        ObterClienteQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterClienteQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
