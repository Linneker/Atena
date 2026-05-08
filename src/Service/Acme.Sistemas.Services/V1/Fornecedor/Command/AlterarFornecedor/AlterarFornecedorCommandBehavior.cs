using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Fornecedor.Command.AlterarFornecedor;

/// <summary>
/// Behavior específico do AlterarFornecedorCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class AlterarFornecedorCommandBehavior
    : IPipelineBehavior<AlterarFornecedorCommand, ResponseDefault<AlterarFornecedorCommandResult>>
{
    public Task<ResponseDefault<AlterarFornecedorCommandResult>> Handle(
        AlterarFornecedorCommand request,
        RequestHandlerDelegate<ResponseDefault<AlterarFornecedorCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
