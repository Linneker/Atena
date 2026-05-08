using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Inventario.Command.FecharInventario;

public sealed record InventarioContagem(Guid ProdutoId, decimal SaldoContado, string? Observacao);

