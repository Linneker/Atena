using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Command.RemoverBeneficioCatalogo;

public sealed class RemoverBeneficioCatalogoCommandBehavior
    : IPipelineBehavior<RemoverBeneficioCatalogoCommand, ResponseDefault<RemoverBeneficioCatalogoCommandResult>>
{
    public Task<ResponseDefault<RemoverBeneficioCatalogoCommandResult>> Handle(
        RemoverBeneficioCatalogoCommand request,
        RequestHandlerDelegate<ResponseDefault<RemoverBeneficioCatalogoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
