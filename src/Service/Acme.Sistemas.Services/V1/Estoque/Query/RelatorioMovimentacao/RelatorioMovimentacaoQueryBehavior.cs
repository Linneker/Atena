using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Estoque.Query.RelatorioMovimentacao;

/// <summary>
/// Behavior específico do RelatorioMovimentacaoQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class RelatorioMovimentacaoQueryBehavior
    : IPipelineBehavior<RelatorioMovimentacaoQuery, ResponseDefault<RelatorioMovimentacaoResult>>
{
    public Task<ResponseDefault<RelatorioMovimentacaoResult>> Handle(
        RelatorioMovimentacaoQuery request,
        RequestHandlerDelegate<ResponseDefault<RelatorioMovimentacaoResult>> next,
        CancellationToken cancellationToken) => next();
}
