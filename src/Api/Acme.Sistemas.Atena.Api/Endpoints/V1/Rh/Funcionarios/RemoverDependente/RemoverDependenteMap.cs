using Acme.Sistemas.Services.V1.Rh.Funcionario.Command.RemoverDependente;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.RemoverDependente;

public static class RemoverDependenteMap
{
    public static RemoverDependenteCommand ToCommand(this RemoverDependenteRequest r) => new(r.DependenteId);
    public static RemoverDependenteResponse ToResponse(this RemoverDependenteCommandResult r) => new(r.DependenteId);
}
