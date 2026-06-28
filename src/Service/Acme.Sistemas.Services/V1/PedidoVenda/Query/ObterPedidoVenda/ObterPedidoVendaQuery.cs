using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.PedidoVenda.Query.ObterPedidoVenda;

public sealed record ObterPedidoVendaQuery(Guid Id) : IRequest<ResponseDefault<ObterPedidoVendaQueryResult>>;
