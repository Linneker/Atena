namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Dividas.AlterarDivida;

public sealed record AlterarDividaRequest(
    string Credor,
    string? Descricao,
    decimal ValorOriginal,
    decimal? TaxaJurosMensal,
    DateTime DataInicio,
    DateTime? DataFim,
    int NumeroParcelas);
