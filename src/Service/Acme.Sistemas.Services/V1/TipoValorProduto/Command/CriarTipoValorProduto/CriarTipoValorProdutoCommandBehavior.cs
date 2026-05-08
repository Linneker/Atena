using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.TipoValorProduto.Command.CriarTipoValorProduto;

/// <summary>
/// Behavior específico do CriarTipoValorProdutoCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class CriarTipoValorProdutoCommandBehavior
    : IPipelineBehavior<CriarTipoValorProdutoCommand, ResponseDefault<CriarTipoValorProdutoCommandResult>>
{
    public Task<ResponseDefault<CriarTipoValorProdutoCommandResult>> Handle(
        CriarTipoValorProdutoCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarTipoValorProdutoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
