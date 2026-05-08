using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Reports;

namespace Acme.Sistemas.Services.V1.Relatorios.Financeiro.Balanco;

/// <summary>
/// Behavior específico do GerarBalancoQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class GerarBalancoQueryBehavior
    : IPipelineBehavior<GerarBalancoQuery, ResponseDefault<BalancoResult>>
{
    public Task<ResponseDefault<BalancoResult>> Handle(
        GerarBalancoQuery request,
        RequestHandlerDelegate<ResponseDefault<BalancoResult>> next,
        CancellationToken cancellationToken) => next();
}
