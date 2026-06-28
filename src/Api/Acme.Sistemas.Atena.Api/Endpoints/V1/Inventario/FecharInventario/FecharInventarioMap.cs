using Acme.Sistemas.Services.V1.Inventario.Command.FecharInventario;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Inventario.FecharInventario;

public static class FecharInventarioMap
{
    public static FecharInventarioCommand ToCommand(this FecharInventarioRequest request, Guid inventarioId)
        => new(inventarioId, request.Contagens);

    public static FecharInventarioResponse ToResponse(this FecharInventarioCommandResult result)
        => new(
            result.InventarioId,
            result.TotalAjustes,
            result.Ajustes.Select(a => a.ToResponseAjuste()).ToArray());

    private static FecharInventarioResponseAjuste ToResponseAjuste(this AjusteGerado ajuste)
        => new(ajuste.ProdutoId, ajuste.SaldoSistema, ajuste.SaldoContado, ajuste.Diferenca);
}
