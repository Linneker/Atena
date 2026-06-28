namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Inventario.AbrirInventario;

public sealed record AbrirInventarioResponse(
    Guid Id,
    Guid EstoqueId,
    int TotalProdutos,
    DateTime DataAbertura);
