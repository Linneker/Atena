using Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Query.ListarBeneficiosCatalogo;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BeneficiosCatalogo.ListarBeneficiosCatalogo;

public static class ListarBeneficiosCatalogoMap
{
    public static ListarBeneficiosCatalogoQuery ToQuery(this ListarBeneficiosCatalogoRequest r)
        => new(r.Skip, r.Take);

    public static ListarBeneficiosCatalogoResponse ToResponse(this ListarBeneficiosCatalogoQueryResult r)
        => new(
            r.Items.Select(i => new ListarBeneficiosCatalogoResponseItem(
                i.Id, i.Codigo, i.Descricao, i.Tipo,
                i.DescontoFuncionarioPct, i.CustoEmpresaPadrao, i.Ativo)).ToList(),
            r.Total);
}
