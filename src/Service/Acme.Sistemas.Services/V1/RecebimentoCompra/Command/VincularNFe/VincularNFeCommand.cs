using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.RecebimentoCompra.Command.VincularNFe;

public sealed record VincularNFeCommand(
    Guid RecebimentoId,
    string NumeroNotaFiscal,
    string ChaveAcesso) : IRequest<ResponseDefault<VincularNFeCommandResult>>;

