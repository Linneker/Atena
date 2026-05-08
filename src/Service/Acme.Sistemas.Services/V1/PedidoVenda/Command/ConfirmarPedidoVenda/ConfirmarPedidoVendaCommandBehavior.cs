using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.PedidoVenda.Command.ConfirmarPedidoVenda;

/// <summary>
/// Behavior específico do ConfirmarPedidoVendaCommand. No-op pass-through — convenção do blueprint
/// (todo Command/Query tem Behavior). Lógica adicional (cache invalidation, etc.)
/// é adicionada quando necessário.
/// </summary>
public sealed class ConfirmarPedidoVendaCommandBehavior
    : IPipelineBehavior<ConfirmarPedidoVendaCommand, ResponseDefault<ConfirmarPedidoVendaCommandResult>>
{
    public Task<ResponseDefault<ConfirmarPedidoVendaCommandResult>> Handle(
        ConfirmarPedidoVendaCommand request,
        RequestHandlerDelegate<ResponseDefault<ConfirmarPedidoVendaCommandResult>> next,
        CancellationToken cancellationToken) => next();
}
