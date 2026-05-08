namespace Acme.Sistemas.Atena.Api.Endpoints.V1.CentrosDeCusto.AlterarCentroDeCusto;

public sealed record AlterarCentroDeCustoRequest(
    string Nome,
    string? Descricao,
    Guid? ResponsavelId,
    bool Ativo);
