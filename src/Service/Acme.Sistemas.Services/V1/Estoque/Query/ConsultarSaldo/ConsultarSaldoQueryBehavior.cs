using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Estoque.Query.ConsultarSaldo;

/// <summary>
/// Behavior específico do ConsultarSaldoQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ConsultarSaldoQueryBehavior
    : IPipelineBehavior<ConsultarSaldoQuery, ResponseDefault<ConsultarSaldoQueryResult>>
{
    public Task<ResponseDefault<ConsultarSaldoQueryResult>> Handle(
        ConsultarSaldoQuery request,
        RequestHandlerDelegate<ResponseDefault<ConsultarSaldoQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
