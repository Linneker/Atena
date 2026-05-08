using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.RecebimentoCompra.Command.VincularNFe;

public sealed record VincularNFeCommandResult(
    Guid RecebimentoId,
    string ChaveAcesso,
    bool ChaveAcessoValida,
    bool ConsultaSefazExecutada);
