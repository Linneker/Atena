using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BeneficiosCatalogo.ObterBeneficioCatalogo;

public sealed record ObterBeneficioCatalogoResponse(
    Guid Id,
    string? Codigo,
    string Descricao,
    TipoBeneficio Tipo,
    decimal? DescontoFuncionarioPct,
    decimal? CustoEmpresaPadrao,
    string? NaturezaRubricaEsocial,
    bool Ativo);
