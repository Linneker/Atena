using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BeneficiosCatalogo.CriarBeneficioCatalogo;

public sealed record CriarBeneficioCatalogoRequest(
    string? Codigo,
    string Descricao,
    TipoBeneficio Tipo,
    decimal? DescontoFuncionarioPct,
    decimal? CustoEmpresaPadrao,
    string? NaturezaRubricaEsocial);
