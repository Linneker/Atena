using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Fiscal.Command.EnviarDanfe;

public sealed record EnviarDanfeCommand(
    Guid NFeId,
    string? EmailDestinoOverride = null) : IRequest<ResponseDefault<EnviarDanfeCommandResult>>;

public sealed record EnviarDanfeCommandResult(Guid NFeId, string EmailDestino);
