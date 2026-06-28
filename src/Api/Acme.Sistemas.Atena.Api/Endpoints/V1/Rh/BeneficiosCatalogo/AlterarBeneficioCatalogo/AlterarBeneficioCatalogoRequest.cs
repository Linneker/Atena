using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BeneficiosCatalogo.AlterarBeneficioCatalogo;

public sealed record AlterarBeneficioCatalogoRequest(
    Guid Id,
    string? Codigo,
    string Descricao,
    TipoBeneficio Tipo,
    decimal? DescontoFuncionarioPct,
    decimal? CustoEmpresaPadrao,
    string? NaturezaRubricaEsocial,
    bool Ativo);
