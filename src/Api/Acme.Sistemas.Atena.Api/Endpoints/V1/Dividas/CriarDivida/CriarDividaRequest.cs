namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Dividas.CriarDivida;

public sealed record CriarDividaRequest(
    string Credor,
    string? Descricao,
    decimal ValorOriginal,
    decimal? TaxaJurosMensal,
    DateTime DataInicio,
    DateTime? DataFim,
    int NumeroParcelas);
