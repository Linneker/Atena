using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Produto.Command.ExcluirProduto;

/// <summary>
/// Behavior específico do ExcluirProdutoCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ExcluirProdutoCommandBehavior
    : IPipelineBehavior<ExcluirProdutoCommand, ResponseDefault>
{
    public Task<ResponseDefault> Handle(
        ExcluirProdutoCommand request,
        RequestHandlerDelegate<ResponseDefault> next,
        CancellationToken cancellationToken) => next();
}
