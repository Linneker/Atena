using Acme.Sistemas.Services.V1.Rh.Departamento.Query.ObterDepartamento;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Departamentos.ObterDepartamento;

public static class ObterDepartamentoMap
{
    public static ObterDepartamentoQuery ToQuery(this ObterDepartamentoRequest r) => new(r.Id);

    public static ObterDepartamentoResponse ToResponse(this ObterDepartamentoQueryResult r)
        => new(r.Id, r.Codigo, r.Nome, r.CentroDeCustoId, r.Ativo);
}
