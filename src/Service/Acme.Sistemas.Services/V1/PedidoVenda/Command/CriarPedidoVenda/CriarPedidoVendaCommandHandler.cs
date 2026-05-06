using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Vendas;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Services.V1.PedidoVenda.Services;
using PedidoEntity = Acme.Sistemas.Domain.Entities.Vendas.PedidoVenda;

namespace Acme.Sistemas.Services.V1.PedidoVenda.Command.CriarPedidoVenda;

public sealed class CriarPedidoVendaCommandHandler
    : IRequestHandler<CriarPedidoVendaCommand, ResponseDefault<CriarPedidoVendaCommandResult>>
{
    private readonly IPedidoVendaRepository _pedidos;
    private readonly IClienteRepository _clientes;
    private readonly IEstoqueProdutoRepository _saldos;
    private readonly ITenantContext _tenantContext;

    public CriarPedidoVendaCommandHandler(
        IPedidoVendaRepository pedidos,
        IClienteRepository clientes,
        IEstoqueProdutoRepository saldos,
        ITenantContext tenantContext)
    {
        _pedidos = pedidos;
        _clientes = clientes;
        _saldos = saldos;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarPedidoVendaCommandResult>> Handle(CriarPedidoVendaCommand request, CancellationToken cancellationToken)
    {
        // 1. Cliente bloqueado para vendas?
        var cliente = await _clientes.GetByIdAsync(request.ClienteId, cancellationToken);
        if (cliente is null)
            return ResponseDefault<CriarPedidoVendaCommandResult>.NotFound("Cliente não encontrado.");
        if (cliente.BloqueadoVendas)
            return ResponseDefault<CriarPedidoVendaCommandResult>.Conflict(
                $"Cliente {cliente.Nome} está bloqueado para vendas.");

        // 2. Alçada de desconto
        if (request.DescontoPercentual is decimal desc && desc > 0
            && !AlcadaDescontoVenda.TemAlcada(_tenantContext.Permissions, desc))
        {
            return ResponseDefault<CriarPedidoVendaCommandResult>.Forbidden(
                $"Sem alçada para desconto de {desc:N2}%. Permissão necessária: {AlcadaDescontoVenda.PermissaoNecessaria(desc)}.");
        }

        // 3. Verifica saldo disponível e reserva (rascunho ainda não reserva — reserva no Confirmar)
        foreach (var item in request.Itens)
        {
            var saldo = await _saldos.GetByEstoqueAndProdutoAsync(request.EstoqueId, item.ProdutoId, cancellationToken);
            var disponivel = saldo?.SaldoDisponivel ?? 0;
            if (item.Quantidade > disponivel)
                return ResponseDefault<CriarPedidoVendaCommandResult>.Conflict(
                    $"Saldo insuficiente do produto {item.ProdutoId} (disponível: {disponivel}, solicitado: {item.Quantidade}).");
        }

        var seq = await _pedidos.NextNumeroAsync(cancellationToken);
        var numero = $"PV{DateTime.UtcNow:yyyyMM}{seq:D5}";
        var subtotal = request.Itens.Sum(i => i.Quantidade * i.PrecoUnitario);
        var valor = subtotal * (1 - (request.DescontoPercentual ?? 0) / 100m);

        var pedido = new PedidoEntity
        {
            TenantId = _tenantContext.TenantId,
            Numero = numero,
            ClienteId = request.ClienteId,
            VendedorId = request.VendedorId,
            EstoqueId = request.EstoqueId,
            OrcamentoId = request.OrcamentoId,
            DataEmissao = DateTime.UtcNow,
            ValorTotal = valor,
            DescontoPercentual = request.DescontoPercentual,
            CondicaoPagamento = request.CondicaoPagamento,
            Observacao = request.Observacao,
            Status = StatusPedidoVenda.Rascunho,
            CreatedBy = _tenantContext.UserId
        };
        await _pedidos.AddAsync(pedido, cancellationToken);

        await _pedidos.AddItensAsync(request.Itens.Select(i => new PedidoVendaItem
        {
            TenantId = _tenantContext.TenantId,
            PedidoVendaId = pedido.Id,
            ProdutoId = i.ProdutoId,
            Quantidade = i.Quantidade,
            PrecoUnitario = i.PrecoUnitario,
            CreatedBy = _tenantContext.UserId
        }), cancellationToken);

        return ResponseDefault<CriarPedidoVendaCommandResult>.Created(
            new CriarPedidoVendaCommandResult(pedido.Id, numero, valor));
    }
}
