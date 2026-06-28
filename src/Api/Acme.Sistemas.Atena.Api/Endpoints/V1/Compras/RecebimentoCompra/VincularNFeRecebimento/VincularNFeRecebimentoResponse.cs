namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.RecebimentoCompra.VincularNFeRecebimento;

public sealed record VincularNFeRecebimentoResponse(
    Guid RecebimentoId,
    string ChaveAcesso,
    bool ChaveAcessoValida,
    bool ConsultaSefazExecutada);
