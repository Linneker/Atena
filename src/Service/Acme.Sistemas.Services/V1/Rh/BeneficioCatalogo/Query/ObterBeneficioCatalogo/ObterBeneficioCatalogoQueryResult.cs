using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Query.ObterBeneficioCatalogo;

public sealed record ObterBeneficioCatalogoQueryResult(
    Guid Id,
    string? Codigo,
    string Descricao,
    TipoBeneficio Tipo,
    decimal? DescontoFuncionarioPct,
    decimal? CustoEmpresaPadrao,
    string? NaturezaRubricaEsocial,
    bool Ativo);
