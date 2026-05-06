using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Inventario.Command.AbrirInventario;

public sealed record AbrirInventarioCommand(
    Guid EstoqueId,
    string? Observacao) : IRequest<ResponseDefault<AbrirInventarioCommandResult>>;

public sealed record AbrirInventarioCommandResult(
    Guid Id, Guid EstoqueId, int TotalProdutos, DateTime DataAbertura);
