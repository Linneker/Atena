using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Compras;
using Acme.Sistemas.Domain.Entities.Estoque;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using ContaPagarEntity = Acme.Sistemas.Domain.Entities.Financeiro.ContaPagar;
using RecebimentoEntity = Acme.Sistemas.Domain.Entities.Compras.RecebimentoCompra;

namespace Acme.Sistemas.Services.V1.RecebimentoCompra.Command.RegistrarRecebimento;

public sealed class RegistrarRecebimentoCommandHandler
    : IRequestHandler<RegistrarRecebimentoCommand, ResponseDefault<RegistrarRecebimentoCommandResult>>
{
    private readonly IRecebimentoCompraRepository _recebimentos;
    private readonly IPedidoCompraRepository _pedidos;
    private readonly IEntradaProdutoEstoqueRepository _entradas;
    private readonly IEstoqueProdutoRepository _saldos;
    private readonly IContaPagarRepository _contasPagar;
    private readonly ITenantContext _tenantContext;

    public RegistrarRecebimentoCommandHandler(
        IRecebimentoCompraRepository recebimentos,
        IPedidoCompraRepository pedidos,
        IEntradaProdutoEstoqueRepository entradas,
        IEstoqueProdutoRepository saldos,
        IContaPagarRepository contasPagar,
        ITenantContext tenantContext)
    {
        _recebimentos = recebimentos;
        _pedidos = pedidos;
        _entradas = entradas;
        _saldos = saldos;
        _contasPagar = contasPagar;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<RegistrarRecebimentoCommandResult>> Handle(RegistrarRecebimentoCommand request, CancellationToken cancellationToken)
    {
        var pedido = await _pedidos.GetByIdAsync(request.PedidoCompraId, cancellationToken);
        if (pedido is null)
            return ResponseDefault<RegistrarRecebimentoCommandResult>.NotFound("Pedido de compra não encontrado.");

        if (pedido.Status == StatusPedidoCompra.Cancelado)
            return ResponseDefault<RegistrarRecebimentoCommandResult>.Conflict("Pedido cancelado.");
        if (pedido.Status == StatusPedidoCompra.Recebido)
            return ResponseDefault<RegistrarRecebimentoCommandResult>.Conflict("Pedido já totalmente recebido.");

        var itensPedido = (await _pedidos.ListItensAsync(pedido.Id, cancellationToken))
            .ToDictionary(i => i.Id, i => i);

        // Valida itens do request
        foreach (var item in request.Itens)
        {
            if (!itensPedido.TryGetValue(item.PedidoCompraItemId, out var itemPedido))
                return ResponseDefault<RegistrarRecebimentoCommandResult>.BadRequest(
                    Core.Response.Erros.Error.Validation($"Item {item.PedidoCompraItemId} não pertence a este pedido."));

            var saldoPendente = itemPedido.Quantidade - itemPedido.QuantidadeRecebida;
            if (item.QuantidadeRecebida > saldoPendente)
                return ResponseDefault<RegistrarRecebimentoCommandResult>.Conflict(
                    $"Quantidade recebida ({item.QuantidadeRecebida}) excede o saldo pendente ({saldoPendente}) do item.");
        }

        var dataRec = request.DataRecebimento ?? DateTime.UtcNow;

        // Determina tipo: detecta divergência (preço diferente do pedido) e parcial vs total
        var temDivergencia = request.Itens.Any(i =>
            i.PrecoUnitario.HasValue && itensPedido.TryGetValue(i.PedidoCompraItemId, out var ip)
            && i.PrecoUnitario.Value != ip.PrecoUnitario);

        var totalRecebidoAposEsta = itensPedido.Values.Sum(ip =>
        {
            var deltaEsta = request.Itens
                .FirstOrDefault(r => r.PedidoCompraItemId == ip.Id)?.QuantidadeRecebida ?? 0;
            return ip.QuantidadeRecebida + deltaEsta;
        });
        var totalPedido = itensPedido.Values.Sum(ip => ip.Quantidade);
        var ehTotal = totalRecebidoAposEsta >= totalPedido;

        var tipo = temDivergencia
            ? TipoRecebimento.ComDivergencia
            : ehTotal ? TipoRecebimento.Total : TipoRecebimento.Parcial;

        var recebimento = new RecebimentoEntity
        {
            TenantId = _tenantContext.TenantId,
            PedidoCompraId = pedido.Id,
            DataRecebimento = dataRec,
            Tipo = tipo,
            NumeroNotaFiscal = request.NumeroNotaFiscal,
            ChaveAcessoNFe = request.ChaveAcessoNFe,
            Observacao = request.Observacao,
            CreatedBy = _tenantContext.UserId
        };
        await _recebimentos.AddAsync(recebimento, cancellationToken);

        var itensReceb = new List<RecebimentoCompraItem>();
        decimal valorTotalRecebido = 0;
        int entradasGeradas = 0;

        foreach (var item in request.Itens)
        {
            var itemPedido = itensPedido[item.PedidoCompraItemId];
            var precoEfetivo = item.PrecoUnitario ?? itemPedido.PrecoUnitario;

            itensReceb.Add(new RecebimentoCompraItem
            {
                TenantId = _tenantContext.TenantId,
                RecebimentoCompraId = recebimento.Id,
                PedidoCompraItemId = item.PedidoCompraItemId,
                ProdutoId = itemPedido.ProdutoId,
                QuantidadeRecebida = item.QuantidadeRecebida,
                PrecoUnitario = precoEfetivo,
                Observacao = item.Observacao,
                CreatedBy = _tenantContext.UserId
            });

            // Atualiza quantidade recebida no item do pedido
            await _recebimentos.UpdatePedidoCompraItemQuantidadeRecebidaAsync(
                itemPedido.Id, itemPedido.QuantidadeRecebida + item.QuantidadeRecebida, cancellationToken);

            // Gera entrada de estoque
            var entrada = new EntradaProdutoEstoque
            {
                TenantId = _tenantContext.TenantId,
                EstoqueId = request.EstoqueId,
                ProdutoId = itemPedido.ProdutoId,
                Quantidade = item.QuantidadeRecebida,
                QuantidadeRestante = item.QuantidadeRecebida,
                CustoUnitario = precoEfetivo,
                Origem = OrigemMovimento.Compra,
                Motivo = $"Recebimento de pedido {pedido.Numero}",
                FornecedorId = pedido.FornecedorId,
                DocumentoReferencia = request.NumeroNotaFiscal ?? pedido.Numero,
                DataMovimento = dataRec,
                CreatedBy = _tenantContext.UserId
            };
            await _entradas.AddAsync(entrada, cancellationToken);

            // Cria ou atualiza saldo
            var saldoAtual = await _saldos.GetByEstoqueAndProdutoAsync(
                request.EstoqueId, itemPedido.ProdutoId, cancellationToken);
            if (saldoAtual is null)
            {
                await _saldos.AddAsync(new EstoqueProduto
                {
                    TenantId = _tenantContext.TenantId,
                    EstoqueId = request.EstoqueId,
                    ProdutoId = itemPedido.ProdutoId,
                    SaldoTotal = item.QuantidadeRecebida,
                    CreatedBy = _tenantContext.UserId
                }, cancellationToken);
            }
            else
            {
                await _saldos.AjustarSaldoAsync(
                    request.EstoqueId, itemPedido.ProdutoId, item.QuantidadeRecebida, 0, cancellationToken);
            }

            valorTotalRecebido += item.QuantidadeRecebida * precoEfetivo;
            entradasGeradas++;
        }

        await _recebimentos.AddItensAsync(itensReceb, cancellationToken);

        // Atualiza status do pedido
        var novoStatusPedido = ehTotal ? StatusPedidoCompra.Recebido : StatusPedidoCompra.RecebimentoParcial;
        await _pedidos.UpdateStatusAsync(pedido.Id, novoStatusPedido, cancellationToken);

        // Gera conta a pagar
        var contaPagar = new ContaPagarEntity
        {
            TenantId = _tenantContext.TenantId,
            Descricao = $"Pedido {pedido.Numero} - Recebimento {recebimento.Id.ToString()[..8]}",
            FornecedorId = pedido.FornecedorId,
            PlanoDeContasId = request.PlanoDeContasId,
            ValorOriginal = valorTotalRecebido,
            DataVencimento = request.VencimentoContaPagar,
            Status = StatusConta.Pendente,
            Observacao = $"Recebimento de pedido de compra {pedido.Numero}" +
                (string.IsNullOrWhiteSpace(request.NumeroNotaFiscal) ? "" : $" - NF {request.NumeroNotaFiscal}"),
            CreatedBy = _tenantContext.UserId
        };
        await _contasPagar.AddAsync(contaPagar, cancellationToken);

        return ResponseDefault<RegistrarRecebimentoCommandResult>.Created(
            new RegistrarRecebimentoCommandResult(
                recebimento.Id, tipo, contaPagar.Id, valorTotalRecebido, entradasGeradas));
    }
}
