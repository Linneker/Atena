using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.CriarFuncionarioCompleto;

public sealed class CriarFuncionarioCompletoCommandBehavior
    : IPipelineBehavior<CriarFuncionarioCompletoCommand, ResponseDefault<CriarFuncionarioCompletoCommandResult>>
{
    public Task<ResponseDefault<CriarFuncionarioCompletoCommandResult>> Handle(
        CriarFuncionarioCompletoCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarFuncionarioCompletoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
