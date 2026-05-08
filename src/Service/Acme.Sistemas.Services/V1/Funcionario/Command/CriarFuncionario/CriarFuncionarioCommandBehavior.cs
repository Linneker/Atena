using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Funcionario.Command.CriarFuncionario;

/// <summary>
/// Behavior específico do CriarFuncionarioCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class CriarFuncionarioCommandBehavior
    : IPipelineBehavior<CriarFuncionarioCommand, ResponseDefault<CriarFuncionarioCommandResult>>
{
    public Task<ResponseDefault<CriarFuncionarioCommandResult>> Handle(
        CriarFuncionarioCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarFuncionarioCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
