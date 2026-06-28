using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Command.CriarBeneficioCatalogo;

public sealed class CriarBeneficioCatalogoCommandBehavior
    : IPipelineBehavior<CriarBeneficioCatalogoCommand, ResponseDefault<CriarBeneficioCatalogoCommandResult>>
{
    public Task<ResponseDefault<CriarBeneficioCatalogoCommandResult>> Handle(
        CriarBeneficioCatalogoCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarBeneficioCatalogoCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
