using Acme.Sistemas.Core.Mediators;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Estoque;
using Acme.Sistemas.Domain.Entities.Vendas;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Services.V1.Faturamento.Events;
using Acme.Sistemas.Services.V1.Faturamento.Services;
using ContaReceberEntity = Acme.Sistemas.Domain.Entities.Financeiro.ContaReceber;
using FaturamentoEntity = Acme.Sistemas.Domain.Entities.Vendas.Faturamento;

namespace Acme.Sistemas.Services.V1.Faturamento.Command.FaturarPedido;

public sealed class FaturarPedidoCommandHandler
    : IRequestHandler<FaturarPedidoCommand, ResponseDefault<FaturarPedidoCommandResult>>
{
    private readonly IFaturamentoRepository _faturamentos;
    private readonly IPedidoVendaRepository _pedidos;
    private readonly IEstoqueProdutoRepository _saldos;
    private readonly ISaidaProdutoEstoqueRepository _saidas;
    private readonly IContaReceberRepository _contasReceber;
    private readonly IComissaoVendedorRepository _comissoes;
    private readonly IMediator _mediator;
    private readonly ITenantContext _tenantContext;

    public FaturarPedidoCommandHandler(
        IFaturamentoRepository faturamentos,
        IPedidoVendaRepository pedidos,
        IEstoqueProdutoRepository saldos,
        ISaidaProdutoEstoqueRepository saidas,
        IContaReceberRepository contasReceber,
        IComissaoVendedorRepository comissoes,
        IMediator mediator,
        ITenantContext tenantContext)
    {
        _faturamentos = faturamentos;
        _pedidos = pedidos;
        _saldos = saldos;
        _saidas = saidas;
        _contasReceber = contasReceber;
        _comissoes = comissoes;
        _mediator = mediator;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<FaturarPedidoCommandResult>> Handle(FaturarPedidoCommand request, CancellationToken cancellationToken)
    {
        var pedido = await _pedidos.GetByIdAsync(request.PedidoVendaId, cancellationToken);
        if (pedido is null)
            return ResponseDefault<FaturarPedidoCommandResult>.NotFound("Pedido não encontrado.");
        if (pedido.Status is StatusPedidoVenda.Cancelado or StatusPedidoVenda.Faturado)
            return ResponseDefault<FaturarPedidoCommandResult>.Conflict($"Pedido em status {pedido.Status}.");
        if (pedido.Status == StatusPedidoVenda.Rascunho)
            return ResponseDefault<FaturarPedidoCommandResult>.Conflict("Pedido deve estar Confirmado para ser faturado.");

        var itensPedido = (await _pedidos.ListItensAsync(pedido.Id, cancellationToken)).ToDictionary(i => i.Id);

        // Valida quantidades
        foreach (var item in request.Itens)
        {
            if (!itensPedido.TryGetValue(item.PedidoVendaItemId, out var ip))
                return ResponseDefault<FaturarPedidoCommandResult>.BadRequest(
                    Core.Response.Erros.Error.Validation($"Item {item.PedidoVendaItemId} não pertence ao pedido."));
            var saldoPendente = ip.Quantidade - ip.QuantidadeFaturada;
            if (item.Quantidade > saldoPendente)
                return ResponseDefault<FaturarPedidoCommandResult>.Conflict(
                    $"Quantidade ({item.Quantidade}) excede pendente ({saldoPendente}) no item {ip.Id}.");
        }

        var totalFaturadoApos = itensPedido.Values.Sum(ip =>
        {
            var delta = request.Itens.FirstOrDefault(r => r.PedidoVendaItemId == ip.Id)?.Quantidade ?? 0;
            return ip.QuantidadeFaturada + delta;
        });
        var totalPedidoQtd = itensPedido.Values.Sum(i => i.Quantidade);
        var faturamentoTotal = totalFaturadoApos >= totalPedidoQtd;

        var seq = await _faturamentos.NextNumeroAsync(cancellationToken);
        var numero = $"FAT{DateTime.UtcNow:yyyyMM}{seq:D5}";

        var fatItens = new List<FaturamentoItem>();
        decimal valorFaturado = 0;

        foreach (var item in request.Itens)
        {
            var ip = itensPedido[item.PedidoVendaItemId];
            fatItens.Add(new FaturamentoItem
            {
                TenantId = _tenantContext.TenantId,
                PedidoVendaItemId = ip.Id,
                ProdutoId = ip.ProdutoId,
                Quantidade = item.Quantidade,
                PrecoUnitario = ip.PrecoUnitario,
                CreatedBy = _tenantContext.UserId
            });
            valorFaturado += item.Quantidade * ip.PrecoUnitario;
        }
        // Aplica desconto do pedido
        valorFaturado *= 1 - (pedido.DescontoPercentual ?? 0) / 100m;

        var faturamento = new FaturamentoEntity
        {
            TenantId = _tenantContext.TenantId,
            Numero = numero,
            PedidoVendaId = pedido.Id,
            DataFaturamento = DateTime.UtcNow,
            Tipo = faturamentoTotal ? TipoFaturamento.Total : TipoFaturamento.Parcial,
            ValorTotal = valorFaturado,
            CreatedBy = _tenantContext.UserId
        };
        await _faturamentos.AddAsync(faturamento, cancellationToken);
        foreach (var fi in fatItens) fi.FaturamentoId = faturamento.Id;
        await _faturamentos.AddItensAsync(fatItens, cancellationToken);

        // Baixa estoque (libera reserva e consome saldo total)
        foreach (var item in request.Itens)
        {
            var ip = itensPedido[item.PedidoVendaItemId];

            await _saidas.AddAsync(new SaidaProdutoEstoque
            {
                TenantId = _tenantContext.TenantId,
                EstoqueId = pedido.EstoqueId,
                ProdutoId = ip.ProdutoId,
                Quantidade = item.Quantidade,
                Origem = OrigemMovimento.Venda,
                Motivo = $"Faturamento {numero} (pedido {pedido.Numero})",
                ClienteId = pedido.ClienteId,
                DocumentoReferencia = numero,
                DataMovimento = faturamento.DataFaturamento,
                CreatedBy = _tenantContext.UserId
            }, cancellationToken);

            // Libera reserva e baixa total
            await _saldos.AjustarSaldoAsync(pedido.EstoqueId, ip.ProdutoId, -item.Quantidade, -item.Quantidade, cancellationToken);

            await _pedidos.UpdateItemQuantidadeFaturadaAsync(
                ip.Id, ip.QuantidadeFaturada + item.Quantidade, cancellationToken);
        }

        // Conta a receber
        var contaReceber = new ContaReceberEntity
        {
            TenantId = _tenantContext.TenantId,
            Descricao = $"Faturamento {numero} - Pedido {pedido.Numero}",
            ClienteId = pedido.ClienteId,
            PlanoDeContasId = request.PlanoDeContasId,
            ValorOriginal = valorFaturado,
            DataVencimento = request.VencimentoContaReceber,
            Status = StatusConta.Pendente,
            CreatedBy = _tenantContext.UserId
        };
        await _contasReceber.AddAsync(contaReceber, cancellationToken);
        await _faturamentos.UpdateContaReceberAsync(faturamento.Id, contaReceber.Id, cancellationToken);

        // Comissão (se vendedor)
        Guid? comissaoId = null;
        if (pedido.VendedorId.HasValue)
        {
            var perc = request.PercentualComissaoOverride ?? ComissaoCalculator.PercentualPadrao;
            var valorComissao = ComissaoCalculator.Calcular(valorFaturado, perc);
            var comissao = new ComissaoVendedor
            {
                TenantId = _tenantContext.TenantId,
                VendedorId = pedido.VendedorId.Value,
                FaturamentoId = faturamento.Id,
                BaseCalculoValor = valorFaturado,
                PercentualComissao = perc,
                ValorComissao = valorComissao,
                DataReferencia = faturamento.DataFaturamento,
                Status = StatusComissao.Pendente,
                CreatedBy = _tenantContext.UserId
            };
            await _comissoes.AddAsync(comissao, cancellationToken);
            comissaoId = comissao.Id;
        }

        // Atualiza status do pedido
        var novoStatusPedido = faturamentoTotal ? StatusPedidoVenda.Faturado : StatusPedidoVenda.FaturamentoParcial;
        await _pedidos.UpdateStatusAsync(pedido.Id, novoStatusPedido, cancellationToken);

        // Dispara emissão NF-e (assíncrono via notification)
        await _mediator.Publish(new NFeEmissaoSolicitadaNotification(
            _tenantContext.TenantId, faturamento.Id, NFeOperacao.Saida, valorFaturado, DateTime.UtcNow),
            cancellationToken);

        return ResponseDefault<FaturarPedidoCommandResult>.Created(
            new FaturarPedidoCommandResult(
                faturamento.Id, numero, valorFaturado, contaReceber.Id, comissaoId, NFeSolicitada: true));
    }
}
