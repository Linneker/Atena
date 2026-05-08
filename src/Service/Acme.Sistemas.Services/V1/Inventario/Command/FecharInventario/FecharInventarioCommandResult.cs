using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Inventario.Command.FecharInventario;

public sealed record FecharInventarioCommand(
    Guid InventarioId,
    IReadOnlyList<InventarioContagem> Contagens) : IRequest<ResponseDefault<FecharInventarioCommandResult>>;

public sealed record AjusteGerado(Guid ProdutoId, decimal SaldoSistema, decimal SaldoContado, decimal Diferenca);

public sealed record FecharInventarioCommandResult(
    Guid InventarioId, int TotalAjustes, IReadOnlyList<AjusteGerado> Ajustes);
