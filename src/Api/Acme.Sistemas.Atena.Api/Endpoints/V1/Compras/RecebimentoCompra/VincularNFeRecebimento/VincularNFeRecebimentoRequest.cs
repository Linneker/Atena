namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.RecebimentoCompra.VincularNFeRecebimento;

public sealed record VincularNFeRecebimentoRequest(
    string NumeroNotaFiscal,
    string ChaveAcesso);
