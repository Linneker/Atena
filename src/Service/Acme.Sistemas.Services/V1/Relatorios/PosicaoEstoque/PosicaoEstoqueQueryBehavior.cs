using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Relatorios.PosicaoEstoque;

/// <summary>
/// Behavior específico do PosicaoEstoqueQuery. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class PosicaoEstoqueQueryBehavior
    : IPipelineBehavior<PosicaoEstoqueQuery, ResponseDefault<PosicaoEstoqueQueryResult>>
{
    public Task<ResponseDefault<PosicaoEstoqueQueryResult>> Handle(
        PosicaoEstoqueQuery request,
        RequestHandlerDelegate<ResponseDefault<PosicaoEstoqueQueryResult>> next,
        CancellationToken cancellationToken) => next();
}
