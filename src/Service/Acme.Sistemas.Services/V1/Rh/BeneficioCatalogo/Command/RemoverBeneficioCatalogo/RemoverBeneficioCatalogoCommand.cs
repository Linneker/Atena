using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Command.RemoverBeneficioCatalogo;

public sealed record RemoverBeneficioCatalogoCommand(Guid Id)
    : IRequest<ResponseDefault<RemoverBeneficioCatalogoCommandResult>>;
