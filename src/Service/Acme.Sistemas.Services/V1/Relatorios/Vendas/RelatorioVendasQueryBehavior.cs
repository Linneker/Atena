using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Relatorios.Vendas;

/// <summary>
/// Behavior específico do RelatorioVendasQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class RelatorioVendasQueryBehavior
    : IPipelineBehavior<RelatorioVendasQuery, ResponseDefault<RelatorioVendasResult>>
{
    public Task<ResponseDefault<RelatorioVendasResult>> Handle(
        RelatorioVendasQuery request,
        RequestHandlerDelegate<ResponseDefault<RelatorioVendasResult>> next,
        CancellationToken cancellationToken) => next();
}
