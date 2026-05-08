using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Receita.Query.ObterReceita;

/// <summary>
/// Behavior específico do ObterReceitaQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ObterReceitaQueryBehavior
    : IPipelineBehavior<ObterReceitaQuery, ResponseDefault<ObterReceitaQueryResult>>
{
    public Task<ResponseDefault<ObterReceitaQueryResult>> Handle(
        ObterReceitaQuery request,
        RequestHandlerDelegate<ResponseDefault<ObterReceitaQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
