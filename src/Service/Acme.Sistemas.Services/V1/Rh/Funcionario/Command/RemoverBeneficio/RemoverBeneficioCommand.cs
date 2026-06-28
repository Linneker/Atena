using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.RemoverBeneficio;

public sealed record RemoverBeneficioCommand(Guid VinculoId)
    : IRequest<ResponseDefault<RemoverBeneficioCommandResult>>;
