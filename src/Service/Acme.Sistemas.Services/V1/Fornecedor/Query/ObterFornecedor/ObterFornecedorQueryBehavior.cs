using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Fornecedor.Query.ObterFornecedor;

/// <summary>
/// Behavior específico do ObterFornecedorQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ObterFornecedorQueryBehavior
    : IPipelineBehavior<ObterFornecedorQuery, ResponseDefault<ObterFornecedorQueryResult>>
{
    public Task<ResponseDefault<ObterFornecedorQueryResult>> Handle(
        ObterFornecedorQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterFornecedorQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
