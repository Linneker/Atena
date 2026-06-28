using Acme.Sistemas.Services.V1.Rh.Departamento.Command.RemoverDepartamento;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Departamentos.RemoverDepartamento;

public static class RemoverDepartamentoMap
{
    public static RemoverDepartamentoCommand ToCommand(this RemoverDepartamentoRequest r) => new(r.Id);
    public static RemoverDepartamentoResponse ToResponse(this RemoverDepartamentoCommandResult r) => new(r.Id);
}
