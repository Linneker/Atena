using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.PedidoCompra.Command.EnviarFornecedor;

/// <summary>
/// Behavior específico do EnviarFornecedorCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class EnviarFornecedorCommandBehavior
    : IPipelineBehavior<EnviarFornecedorCommand, ResponseDefault<EnviarFornecedorCommandResult>>
{
    public Task<ResponseDefault<EnviarFornecedorCommandResult>> Handle(
        EnviarFornecedorCommand request,
        RequestHandlerDelegate<ResponseDefault<EnviarFornecedorCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
