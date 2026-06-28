using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.AlterarFuncionarioDados;

public sealed class AlterarFuncionarioDadosCommandBehavior
    : IPipelineBehavior<AlterarFuncionarioDadosCommand, ResponseDefault<AlterarFuncionarioDadosCommandResult>>
{
    public Task<ResponseDefault<AlterarFuncionarioDadosCommandResult>> Handle(
        AlterarFuncionarioDadosCommand request,
        RequestHandlerDelegate<ResponseDefault<AlterarFuncionarioDadosCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
