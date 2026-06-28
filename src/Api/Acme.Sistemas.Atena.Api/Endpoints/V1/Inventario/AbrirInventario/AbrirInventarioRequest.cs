namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Inventario.AbrirInventario;

public sealed record AbrirInventarioRequest(
    Guid EstoqueId,
    string? Observacao);
