using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Funcionario.Command.ExcluirFuncionario;

/// <summary>
/// Behavior específico do ExcluirFuncionarioCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ExcluirFuncionarioCommandBehavior
    : IPipelineBehavior<ExcluirFuncionarioCommand, ResponseDefault>
{
    public Task<ResponseDefault> Handle(
        ExcluirFuncionarioCommand request,
        RequestHandlerDelegate<ResponseDefault> next,
        CancellationToken cancellationToken) => next();
}
