namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Departamentos.CriarDepartamento;

public sealed record CriarDepartamentoRequest(
    string? Codigo,
    string Nome,
    Guid? CentroDeCustoId);
