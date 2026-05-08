using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.PlanoDeContas.Query.ListarPlanoDeContas;

/// <summary>
/// Behavior específico do ListarPlanoDeContasQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ListarPlanoDeContasQueryBehavior
    : IPipelineBehavior<ListarPlanoDeContasQuery, ResponseDefault<ListarPlanoDeContasQueryResult>>
{
    public Task<ResponseDefault<ListarPlanoDeContasQueryResult>> Handle(
        ListarPlanoDeContasQuery request,
        RequestHandlerDelegate<ResponseDefault<ListarPlanoDeContasQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
