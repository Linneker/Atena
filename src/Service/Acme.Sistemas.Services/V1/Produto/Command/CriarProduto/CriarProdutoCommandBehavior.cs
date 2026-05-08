using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Produto.Command.CriarProduto;

/// <summary>
/// Behavior específico do CriarProdutoCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class CriarProdutoCommandBehavior
    : IPipelineBehavior<CriarProdutoCommand, ResponseDefault<CriarProdutoCommandResult>>
{
    public Task<ResponseDefault<CriarProdutoCommandResult>> Handle(
        CriarProdutoCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarProdutoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
