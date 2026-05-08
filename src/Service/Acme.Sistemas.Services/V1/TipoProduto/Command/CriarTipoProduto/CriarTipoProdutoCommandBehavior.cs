using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.TipoProduto.Command.CriarTipoProduto;

/// <summary>
/// Behavior específico do CriarTipoProdutoCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class CriarTipoProdutoCommandBehavior
    : IPipelineBehavior<CriarTipoProdutoCommand, ResponseDefault<CriarTipoProdutoCommandResult>>
{
    public Task<ResponseDefault<CriarTipoProdutoCommandResult>> Handle(
        CriarTipoProdutoCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarTipoProdutoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
