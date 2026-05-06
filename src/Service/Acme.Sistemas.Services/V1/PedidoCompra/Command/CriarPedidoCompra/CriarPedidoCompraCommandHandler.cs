using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Compras;
using Acme.Sistemas.Domain.Interfaces.Repository;
using PedidoEntity = Acme.Sistemas.Domain.Entities.Compras.PedidoCompra;

namespace Acme.Sistemas.Services.V1.PedidoCompra.Command.CriarPedidoCompra;

public sealed class CriarPedidoCompraCommandHandler
    : IRequestHandler<CriarPedidoCompraCommand, ResponseDefault<CriarPedidoCompraCommandResult>>
{
    private readonly IPedidoCompraRepository _pedidos;
    private readonly ISolicitacaoCompraRepository _solicitacoes;
    private readonly IFornecedorRepository _fornecedores;
    private readonly ITenantContext _tenantContext;

    public CriarPedidoCompraCommandHandler(
        IPedidoCompraRepository pedidos,
        ISolicitacaoCompraRepository solicitacoes,
        IFornecedorRepository fornecedores,
        ITenantContext tenantContext)
    {
        _pedidos = pedidos;
        _solicitacoes = solicitacoes;
        _fornecedores = fornecedores;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarPedidoCompraCommandResult>> Handle(CriarPedidoCompraCommand request, CancellationToken cancellationToken)
    {
        var fornecedor = await _fornecedores.GetByIdAsync(request.FornecedorId, cancellationToken);
        if (fornecedor is null)
            return ResponseDefault<CriarPedidoCompraCommandResult>.NotFound("Fornecedor não encontrado.");

        List<PedidoCompraItem> itensPedido;
        Guid? solicitacaoIdFinal = request.SolicitacaoCompraId;

        if (request.SolicitacaoCompraId.HasValue)
        {
            var solicitacao = await _solicitacoes.GetByIdAsync(request.SolicitacaoCompraId.Value, cancellationToken);
            if (solicitacao is null)
                return ResponseDefault<CriarPedidoCompraCommandResult>.NotFound("Solicitação de compra não encontrada.");

            if (solicitacao.Status != StatusSolicitacaoCompra.Aprovada)
                return ResponseDefault<CriarPedidoCompraCommandResult>.Conflict(
                    $"Solicitação não está aprovada (status atual: {solicitacao.Status}).");

            var itensSol = await _solicitacoes.ListItensAsync(solicitacao.Id, cancellationToken);
            // Se itens vieram explicitamente no request (substituem preços), usa eles; senão, mapeia do solicitacao
            itensPedido = (request.Itens?.Count > 0
                ? request.Itens.Select(i => new PedidoCompraItem
                {
                    TenantId = _tenantContext.TenantId,
                    ProdutoId = i.ProdutoId,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario,
                    CreatedBy = _tenantContext.UserId
                })
                : itensSol.Select(s => new PedidoCompraItem
                {
                    TenantId = _tenantContext.TenantId,
                    ProdutoId = s.ProdutoId,
                    Quantidade = s.Quantidade,
                    PrecoUnitario = s.PrecoEstimado ?? 0,
                    CreatedBy = _tenantContext.UserId
                })).ToList();
        }
        else
        {
            // Pedido direto
            itensPedido = request.Itens!.Select(i => new PedidoCompraItem
            {
                TenantId = _tenantContext.TenantId,
                ProdutoId = i.ProdutoId,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario,
                CreatedBy = _tenantContext.UserId
            }).ToList();
        }

        if (itensPedido.Count == 0)
            return ResponseDefault<CriarPedidoCompraCommandResult>.BadRequest(
                Core.Response.Erros.Error.Validation("Pedido sem itens."));

        var seq = await _pedidos.NextNumeroAsync(cancellationToken);
        var numero = $"PC{DateTime.UtcNow:yyyyMM}{seq:D5}";
        var valorTotal = itensPedido.Sum(i => i.Quantidade * i.PrecoUnitario);

        var pedido = new PedidoEntity
        {
            TenantId = _tenantContext.TenantId,
            Numero = numero,
            FornecedorId = request.FornecedorId,
            SolicitacaoCompraId = solicitacaoIdFinal,
            DataEmissao = DateTime.UtcNow,
            PrevisaoEntrega = request.PrevisaoEntrega,
            CondicaoPagamento = request.CondicaoPagamento ?? fornecedor.CondicaoPagamentoPadrao,
            ValorTotal = valorTotal,
            Status = StatusPedidoCompra.Rascunho,
            Observacao = request.Observacao,
            CreatedBy = _tenantContext.UserId
        };

        await _pedidos.AddAsync(pedido, cancellationToken);
        foreach (var item in itensPedido)
            item.PedidoCompraId = pedido.Id;
        await _pedidos.AddItensAsync(itensPedido, cancellationToken);

        // Marca solicitacao como ConvertidaEmPedido (preservando aprovado_por/em)
        if (solicitacaoIdFinal.HasValue)
        {
            await _solicitacoes.UpdateStatusOnlyAsync(
                solicitacaoIdFinal.Value, StatusSolicitacaoCompra.ConvertidaEmPedido, cancellationToken);
        }

        return ResponseDefault<CriarPedidoCompraCommandResult>.Created(
            new CriarPedidoCompraCommandResult(pedido.Id, pedido.Numero, valorTotal));
    }
}
