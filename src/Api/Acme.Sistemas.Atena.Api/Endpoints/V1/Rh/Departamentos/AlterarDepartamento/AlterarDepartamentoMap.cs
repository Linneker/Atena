using Acme.Sistemas.Services.V1.Rh.Departamento.Command.AlterarDepartamento;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Departamentos.AlterarDepartamento;

public static class AlterarDepartamentoMap
{
    public static AlterarDepartamentoCommand ToCommand(this AlterarDepartamentoRequest r)
        => new(r.Id, r.Codigo, r.Nome, r.CentroDeCustoId, r.Ativo);

    public static AlterarDepartamentoResponse ToResponse(this AlterarDepartamentoCommandResult r)
        => new(r.Id);
}
