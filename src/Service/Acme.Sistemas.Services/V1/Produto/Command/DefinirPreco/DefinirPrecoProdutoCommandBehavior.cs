using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Produto.Command.DefinirPreco;

/// <summary>
/// Behavior específico do DefinirPrecoProdutoCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class DefinirPrecoProdutoCommandBehavior
    : IPipelineBehavior<DefinirPrecoProdutoCommand, ResponseDefault<DefinirPrecoProdutoCommandResult>>
{
    public Task<ResponseDefault<DefinirPrecoProdutoCommandResult>> Handle(
        DefinirPrecoProdutoCommand request,
        RequestHandlerDelegate<ResponseDefault<DefinirPrecoProdutoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
