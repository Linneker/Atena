namespace Acme.Sistemas.Services.V1.Rh.Departamento.Query.ObterDepartamento;

public sealed record ObterDepartamentoQueryResult(
    Guid Id,
    string? Codigo,
    string Nome,
    Guid? CentroDeCustoId,
    bool Ativo);
