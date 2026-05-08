namespace Acme.Sistemas.Atena.Api.Endpoints.V1.CentrosDeCusto.CriarCentroDeCusto;

public sealed record CriarCentroDeCustoResponse(
    Guid Id,
    string Codigo,
    string Nome);
