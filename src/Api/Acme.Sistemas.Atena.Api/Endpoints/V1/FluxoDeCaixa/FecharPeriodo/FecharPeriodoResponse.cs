namespace Acme.Sistemas.Atena.Api.Endpoints.V1.FluxoDeCaixa.FecharPeriodo;

public sealed record FecharPeriodoResponse(
    Guid Id,
    int Ano,
    int Mes,
    decimal TotalReceitas,
    decimal TotalDespesas,
    decimal Resultado,
    DateTime FechadoEm);
