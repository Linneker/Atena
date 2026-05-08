using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Dashboard.Query.EvolucaoFinanceira;

/// <summary>
/// Behavior específico do EvolucaoFinanceiraQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class EvolucaoFinanceiraQueryBehavior
    : IPipelineBehavior<EvolucaoFinanceiraQuery, ResponseDefault<EvolucaoFinanceiraQueryResult>>
{
    public Task<ResponseDefault<EvolucaoFinanceiraQueryResult>> Handle(
        EvolucaoFinanceiraQuery request,
        RequestHandlerDelegate<ResponseDefault<EvolucaoFinanceiraQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
