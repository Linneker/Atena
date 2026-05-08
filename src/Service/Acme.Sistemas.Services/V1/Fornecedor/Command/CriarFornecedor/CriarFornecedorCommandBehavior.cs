using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Fornecedor.Command.CriarFornecedor;

/// <summary>
/// Behavior específico do CriarFornecedorCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class CriarFornecedorCommandBehavior
    : IPipelineBehavior<CriarFornecedorCommand, ResponseDefault<CriarFornecedorCommandResult>>
{
    public Task<ResponseDefault<CriarFornecedorCommandResult>> Handle(
        CriarFornecedorCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarFornecedorCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
