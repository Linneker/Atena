using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Vendas;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.PedidoVenda.Command.ConfirmarPedidoVenda;

public sealed class ConfirmarPedidoVendaCommandHandler
    : IRequestHandler<ConfirmarPedidoVendaCommand, ResponseDefault<ConfirmarPedidoVendaCommandResult>>
{
    private readonly IPedidoVendaRepository _pedidos;
    private readonly IEstoqueProdutoRepository _saldos;

    public ConfirmarPedidoVendaCommandHandler(IPedidoVendaRepository pedidos, IEstoqueProdutoRepository saldos)
    {
        _pedidos = pedidos;
        _saldos = saldos;
    }

    public async Task<ResponseDefault<ConfirmarPedidoVendaCommandResult>> Handle(ConfirmarPedidoVendaCommand request, CancellationToken cancellationToken)
    {
        var pedido = await _pedidos.GetByIdAsync(request.Id, cancellationToken);
        if (pedido is null)
            return ResponseDefault<ConfirmarPedidoVendaCommandResult>.NotFound("Pedido não encontrado.");
        if (pedido.Status != StatusPedidoVenda.Rascunho)
            return ResponseDefault<ConfirmarPedidoVendaCommandResult>.Conflict(
                $"Apenas pedidos em Rascunho podem ser confirmados (status atual: {pedido.Status}).");

        var itens = await _pedidos.ListItensAsync(pedido.Id, cancellationToken);
        decimal totalReservado = 0;

        // Revalida disponibilidade e reserva atomicamente
        foreach (var item in itens)
        {
            var saldo = await _saldos.GetByEstoqueAndProdutoAsync(pedido.EstoqueId, item.ProdutoId, cancellationToken);
            var disponivel = saldo?.SaldoDisponivel ?? 0;
            if (item.Quantidade > disponivel)
                return ResponseDefault<ConfirmarPedidoVendaCommandResult>.Conflict(
                    $"Saldo insuficiente para confirmar produto {item.ProdutoId} (disponível: {disponivel}).");
        }

        // Reserva (incrementa saldo_reservado)
        foreach (var item in itens)
        {
            await _saldos.AjustarSaldoAsync(pedido.EstoqueId, item.ProdutoId, 0, item.Quantidade, cancellationToken);
            totalReservado += item.Quantidade;
        }

        await _pedidos.UpdateStatusAsync(pedido.Id, StatusPedidoVenda.Confirmado, cancellationToken);

        return ResponseDefault<ConfirmarPedidoVendaCommandResult>.Ok(
            new ConfirmarPedidoVendaCommandResult(pedido.Id, totalReservado));
    }
}
