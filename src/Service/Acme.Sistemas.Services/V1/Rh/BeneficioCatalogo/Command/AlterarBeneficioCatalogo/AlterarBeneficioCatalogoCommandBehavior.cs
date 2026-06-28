using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Command.AlterarBeneficioCatalogo;

public sealed class AlterarBeneficioCatalogoCommandBehavior
    : IPipelineBehavior<AlterarBeneficioCatalogoCommand, ResponseDefault<AlterarBeneficioCatalogoCommandResult>>
{
    public Task<ResponseDefault<AlterarBeneficioCatalogoCommandResult>> Handle(
        AlterarBeneficioCatalogoCommand request,
        RequestHandlerDelegate<ResponseDefault<AlterarBeneficioCatalogoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
