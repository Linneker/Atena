using Acme.Sistemas.Services.V1.Inventario.Command.FecharInventario;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Inventario.FecharInventario;

public sealed record FecharInventarioRequest(
    IReadOnlyList<InventarioContagem> Contagens);
