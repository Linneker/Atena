using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.VincularBeneficio;

public sealed class VincularBeneficioCommandBehavior
    : IPipelineBehavior<VincularBeneficioCommand, ResponseDefault<VincularBeneficioCommandResult>>
{
    public Task<ResponseDefault<VincularBeneficioCommandResult>> Handle(
        VincularBeneficioCommand request,
        RequestHandlerDelegate<ResponseDefault<VincularBeneficioCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
