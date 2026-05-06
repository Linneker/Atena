using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Fiscal;

namespace Acme.Sistemas.Services.V1.Fiscal.Command.AlterarAmbiente;

public sealed record AlterarAmbienteCommand(AmbienteFiscal Ambiente)
    : IRequest<ResponseDefault<AlterarAmbienteCommandResult>>;

public sealed record AlterarAmbienteCommandResult(AmbienteFiscal Ambiente, DateTime AlteradoEm);
