using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Reports;

namespace Acme.Sistemas.Services.V1.Relatorios.Financeiro.DRE;

/// <summary>
/// Behavior específico do GerarDREQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class GerarDREQueryBehavior
    : IPipelineBehavior<GerarDREQuery, ResponseDefault<DREResult>>
{
    public Task<ResponseDefault<DREResult>> Handle(
        GerarDREQuery request,
        RequestHandlerDelegate<ResponseDefault<DREResult>> next,
        CancellationToken cancellationToken) => next();
}
