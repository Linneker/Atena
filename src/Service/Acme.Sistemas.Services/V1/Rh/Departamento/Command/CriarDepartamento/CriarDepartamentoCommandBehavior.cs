using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Departamento.Command.CriarDepartamento;

public sealed class CriarDepartamentoCommandBehavior
    : IPipelineBehavior<CriarDepartamentoCommand, ResponseDefault<CriarDepartamentoCommandResult>>
{
    public Task<ResponseDefault<CriarDepartamentoCommandResult>> Handle(
        CriarDepartamentoCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarDepartamentoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
