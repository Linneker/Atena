namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FluxoDeCaixa.FecharPeriodo;

public sealed record FecharPeriodoRequest(
    int Ano,
    int Mes,
    string? Observacao);
