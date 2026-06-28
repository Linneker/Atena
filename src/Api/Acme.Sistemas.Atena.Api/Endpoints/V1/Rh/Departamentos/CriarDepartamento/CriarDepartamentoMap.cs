using Acme.Sistemas.Services.V1.Rh.Departamento.Command.CriarDepartamento;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Departamentos.CriarDepartamento;

public static class CriarDepartamentoMap
{
    public static CriarDepartamentoCommand ToCommand(this CriarDepartamentoRequest r)
        => new(r.Codigo, r.Nome, r.CentroDeCustoId);

    public static CriarDepartamentoResponse ToResponse(this CriarDepartamentoCommandResult r)
        => new(r.Id, r.Nome);
}
