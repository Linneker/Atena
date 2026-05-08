using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.PedidoCompra.Command.EnviarFornecedor;

public sealed record EnviarFornecedorCommandResult(
    Guid PedidoId, string EmailDestino, DateTime EnviadoEm);
