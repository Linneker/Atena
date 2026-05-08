using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.PedidoCompra.Command.EnviarFornecedor;

public sealed record EnviarFornecedorCommand(
    Guid PedidoId,
    string? EmailDestinoOverride = null) : IRequest<ResponseDefault<EnviarFornecedorCommandResult>>;

