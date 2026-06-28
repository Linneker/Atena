namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Departamentos.AlterarDepartamento;

public sealed record AlterarDepartamentoRequest(
    Guid Id,
    string? Codigo,
    string Nome,
    Guid? CentroDeCustoId,
    bool Ativo);
