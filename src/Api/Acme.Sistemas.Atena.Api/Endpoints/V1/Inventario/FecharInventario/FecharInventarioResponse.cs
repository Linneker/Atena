namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Inventario.FecharInventario;

public sealed record FecharInventarioResponseAjuste(
    Guid ProdutoId,
    decimal SaldoSistema,
    decimal SaldoContado,
    decimal Diferenca);

public sealed record FecharInventarioResponse(
    Guid InventarioId,
    int TotalAjustes,
    IReadOnlyList<FecharInventarioResponseAjuste> Ajustes);
