using Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Command.RemoverBeneficioCatalogo;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BeneficiosCatalogo.RemoverBeneficioCatalogo;

public static class RemoverBeneficioCatalogoMap
{
    public static RemoverBeneficioCatalogoCommand ToCommand(this RemoverBeneficioCatalogoRequest r) => new(r.Id);
    public static RemoverBeneficioCatalogoResponse ToResponse(this RemoverBeneficioCatalogoCommandResult r) => new(r.Id);
}
