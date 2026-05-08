using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Funcionario.Command.AlterarFuncionario;

/// <summary>
/// Behavior específico do AlterarFuncionarioCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class AlterarFuncionarioCommandBehavior
    : IPipelineBehavior<AlterarFuncionarioCommand, ResponseDefault<AlterarFuncionarioCommandResult>>
{
    public Task<ResponseDefault<AlterarFuncionarioCommandResult>> Handle(
        AlterarFuncionarioCommand request,
        RequestHandlerDelegate<ResponseDefault<AlterarFuncionarioCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
