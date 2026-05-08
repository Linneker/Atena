using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Fiscal.Command.CancelarNFe;

public sealed record CancelarNFeCommand(
    Guid NFeId,
    string Justificativa) : IRequest<ResponseDefault<CancelarNFeCommandResult>>;

