namespace Acme.Sistemas.Atena.Api.Endpoints.V1.CentrosDeCusto.CriarCentroDeCusto;

public sealed record CriarCentroDeCustoRequest(
    string Codigo,
    string Nome,
    string? Descricao,
    Guid? ResponsavelId);
