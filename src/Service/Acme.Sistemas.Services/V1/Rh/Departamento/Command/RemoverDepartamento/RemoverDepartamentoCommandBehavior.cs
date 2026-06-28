using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Departamento.Command.RemoverDepartamento;

public sealed class RemoverDepartamentoCommandBehavior
    : IPipelineBehavior<RemoverDepartamentoCommand, ResponseDefault<RemoverDepartamentoCommandResult>>
{
    public Task<ResponseDefault<RemoverDepartamentoCommandResult>> Handle(
        RemoverDepartamentoCommand request,
        RequestHandlerDelegate<ResponseDefault<RemoverDepartamentoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
