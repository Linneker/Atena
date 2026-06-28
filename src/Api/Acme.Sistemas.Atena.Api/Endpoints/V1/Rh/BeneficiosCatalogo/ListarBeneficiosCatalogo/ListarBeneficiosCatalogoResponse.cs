using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.BeneficiosCatalogo.ListarBeneficiosCatalogo;

public sealed record ListarBeneficiosCatalogoResponseItem(
    Guid Id,
    string? Codigo,
    string Descricao,
    TipoBeneficio Tipo,
    decimal? DescontoFuncionarioPct,
    decimal? CustoEmpresaPadrao,
    bool Ativo);

public sealed record ListarBeneficiosCatalogoResponse(
    IReadOnlyList<ListarBeneficiosCatalogoResponseItem> Items,
    long Total);
