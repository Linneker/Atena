using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.RemoverBeneficio;

public sealed class RemoverBeneficioCommandBehavior
    : IPipelineBehavior<RemoverBeneficioCommand, ResponseDefault<RemoverBeneficioCommandResult>>
{
    public Task<ResponseDefault<RemoverBeneficioCommandResult>> Handle(
        RemoverBeneficioCommand request,
        RequestHandlerDelegate<ResponseDefault<RemoverBeneficioCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
