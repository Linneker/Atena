using Acme.Sistemas.Services.V1.RecebimentoCompra.Command.VincularNFe;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.RecebimentoCompra.VincularNFeRecebimento;

public static class VincularNFeRecebimentoMap
{
    public static VincularNFeCommand ToCommand(this VincularNFeRecebimentoRequest request, Guid recebimentoId)
        => new(recebimentoId, request.NumeroNotaFiscal, request.ChaveAcesso);

    public static VincularNFeRecebimentoResponse ToResponse(this VincularNFeCommandResult result)
        => new(result.RecebimentoId, result.ChaveAcesso, result.ChaveAcessoValida, result.ConsultaSefazExecutada);
}
