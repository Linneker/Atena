using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.PedidoVenda.Command.CriarPedidoVenda;

/// <summary>
/// Behavior específico do CriarPedidoVendaCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class CriarPedidoVendaCommandBehavior
    : IPipelineBehavior<CriarPedidoVendaCommand, ResponseDefault<CriarPedidoVendaCommandResult>>
{
    public Task<ResponseDefault<CriarPedidoVendaCommandResult>> Handle(
        CriarPedidoVendaCommand request,
        RequestHandlerDelegate<ResponseDefault<CriarPedidoVendaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
