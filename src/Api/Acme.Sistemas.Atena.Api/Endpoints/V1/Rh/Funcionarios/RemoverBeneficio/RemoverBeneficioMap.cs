using Acme.Sistemas.Services.V1.Rh.Funcionario.Command.RemoverBeneficio;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.RemoverBeneficio;

public static class RemoverBeneficioMap
{
    public static RemoverBeneficioCommand ToCommand(this RemoverBeneficioRequest r) => new(r.VinculoId);
    public static RemoverBeneficioResponse ToResponse(this RemoverBeneficioCommandResult r) => new(r.VinculoId);
}
