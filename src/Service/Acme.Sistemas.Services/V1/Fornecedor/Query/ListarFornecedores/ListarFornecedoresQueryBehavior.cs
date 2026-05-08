using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Fornecedor.Query.ListarFornecedores;

/// <summary>
/// Behavior específico do ListarFornecedoresQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ListarFornecedoresQueryBehavior
    : IPipelineBehavior<ListarFornecedoresQuery, ResponseDefault<ListarFornecedoresQueryResult>>
{
    public Task<ResponseDefault<ListarFornecedoresQueryResult>> Handle(
        ListarFornecedoresQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarFornecedoresQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
