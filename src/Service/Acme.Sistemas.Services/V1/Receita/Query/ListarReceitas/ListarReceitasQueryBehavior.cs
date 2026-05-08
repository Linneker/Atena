using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Receita.Query.ListarReceitas;

/// <summary>
/// Behavior específico do ListarReceitasQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ListarReceitasQueryBehavior
    : IPipelineBehavior<ListarReceitasQuery, ResponseDefault<ListarReceitasQueryResult>>
{
    public Task<ResponseDefault<ListarReceitasQueryResult>> Handle(
        ListarReceitasQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarReceitasQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
