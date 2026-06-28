using Acme.Sistemas.Services.V1.Inventario.Command.AbrirInventario;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Inventario.AbrirInventario;

public static class AbrirInventarioMap
{
    public static AbrirInventarioCommand ToCommand(this AbrirInventarioRequest request)
        => new(request.EstoqueId, request.Observacao);

    public static AbrirInventarioResponse ToResponse(this AbrirInventarioCommandResult result)
        => new(result.Id, result.EstoqueId, result.TotalProdutos, result.DataAbertura);
}
