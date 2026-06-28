using Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Command.CriarBeneficioCatalogo;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BeneficiosCatalogo.CriarBeneficioCatalogo;

public static class CriarBeneficioCatalogoMap
{
    public static CriarBeneficioCatalogoCommand ToCommand(this CriarBeneficioCatalogoRequest r)
        => new(r.Codigo, r.Descricao, r.Tipo,
               r.DescontoFuncionarioPct, r.CustoEmpresaPadrao, r.NaturezaRubricaEsocial);

    public static CriarBeneficioCatalogoResponse ToResponse(this CriarBeneficioCatalogoCommandResult r)
        => new(r.Id, r.Descricao);
}
