namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Dividas.CriarDivida;

public sealed record CriarDividaResponse(Guid Id, string Credor, decimal ValorOriginal);
