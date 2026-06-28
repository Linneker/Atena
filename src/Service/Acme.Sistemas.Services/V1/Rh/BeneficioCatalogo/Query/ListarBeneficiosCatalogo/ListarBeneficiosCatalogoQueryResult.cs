using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.BeneficioCatalogo.Query.ListarBeneficiosCatalogo;

public sealed record ListarBeneficiosCatalogoQueryItem(
    Guid Id,
    string? Codigo,
    string Descricao,
    TipoBeneficio Tipo,
    decimal? DescontoFuncionarioPct,
    decimal? CustoEmpresaPadrao,
    bool Ativo);

public sealed record ListarBeneficiosCatalogoQueryResult(
    IReadOnlyList<ListarBeneficiosCatalogoQueryItem> Items,
    long Total);
