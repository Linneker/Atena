using Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Command.AlterarBeneficioCatalogo;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BeneficiosCatalogo.AlterarBeneficioCatalogo;

public static class AlterarBeneficioCatalogoMap
{
    public static AlterarBeneficioCatalogoCommand ToCommand(this AlterarBeneficioCatalogoRequest r)
        => new(r.Id, r.Codigo, r.Descricao, r.Tipo,
               r.DescontoFuncionarioPct, r.CustoEmpresaPadrao,
               r.NaturezaRubricaEsocial, r.Ativo);

    public static AlterarBeneficioCatalogoResponse ToResponse(this AlterarBeneficioCatalogoCommandResult r)
        => new(r.Id);
}
