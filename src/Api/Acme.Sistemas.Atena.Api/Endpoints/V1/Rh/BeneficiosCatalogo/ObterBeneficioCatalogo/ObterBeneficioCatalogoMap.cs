using Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Query.ObterBeneficioCatalogo;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BeneficiosCatalogo.ObterBeneficioCatalogo;

public static class ObterBeneficioCatalogoMap
{
    public static ObterBeneficioCatalogoQuery ToQuery(this ObterBeneficioCatalogoRequest r) => new(r.Id);

    public static ObterBeneficioCatalogoResponse ToResponse(this ObterBeneficioCatalogoQueryResult r)
        => new(r.Id, r.Codigo, r.Descricao, r.Tipo,
               r.DescontoFuncionarioPct, r.CustoEmpresaPadrao,
               r.NaturezaRubricaEsocial, r.Ativo);
}
