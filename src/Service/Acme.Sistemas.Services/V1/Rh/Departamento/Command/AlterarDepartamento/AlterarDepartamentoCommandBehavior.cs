using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Departamento.Command.AlterarDepartamento;

public sealed class AlterarDepartamentoCommandBehavior
    : IPipelineBehavior<AlterarDepartamentoCommand, ResponseDefault<AlterarDepartamentoCommandResult>>
{
    public Task<ResponseDefault<AlterarDepartamentoCommandResult>> Handle(
        AlterarDepartamentoCommand request,
        RequestHandlerDelegate<ResponseDefault<AlterarDepartamentoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
