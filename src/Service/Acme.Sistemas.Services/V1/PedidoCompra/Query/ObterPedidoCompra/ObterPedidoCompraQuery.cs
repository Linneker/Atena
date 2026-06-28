using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.PedidoCompra.Query.ObterPedidoCompra;

public sealed record ObterPedidoCompraQuery(Guid Id) : IRequest<ResponseDefault<ObterPedidoCompraQueryResult>>;
