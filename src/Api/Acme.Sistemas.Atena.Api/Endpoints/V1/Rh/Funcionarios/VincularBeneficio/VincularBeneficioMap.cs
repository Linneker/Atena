using Acme.Sistemas.Services.V1.Rh.Funcionario.Command.VincularBeneficio;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.VincularBeneficio;

public static class VincularBeneficioMap
{
    public static VincularBeneficioCommand ToCommand(this VincularBeneficioRequest r)
        => new(r.FuncionarioId, r.BeneficioCatalogoId, r.Valor,
               r.DescontoFuncionarioPct, r.VigenciaInicio, r.Observacao);

    public static VincularBeneficioResponse ToResponse(this VincularBeneficioCommandResult r)
        => new(r.Id);
}
