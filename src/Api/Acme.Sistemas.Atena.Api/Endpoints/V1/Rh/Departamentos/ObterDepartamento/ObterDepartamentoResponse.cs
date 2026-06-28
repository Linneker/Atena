namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Departamentos.ObterDepartamento;

public sealed record ObterDepartamentoResponse(
    Guid Id,
    string? Codigo,
    string Nome,
    Guid? CentroDeCustoId,
    bool Ativo);
