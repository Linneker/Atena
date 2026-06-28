using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.AlterarFuncionarioContrato;

public sealed class AlterarFuncionarioContratoCommandBehavior
    : IPipelineBehavior<AlterarFuncionarioContratoCommand, ResponseDefault<AlterarFuncionarioContratoCommandResult>>
{
    public Task<ResponseDefault<AlterarFuncionarioContratoCommandResult>> Handle(
        AlterarFuncionarioContratoCommand request,
        RequestHandlerDelegate<ResponseDefault<AlterarFuncionarioContratoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
