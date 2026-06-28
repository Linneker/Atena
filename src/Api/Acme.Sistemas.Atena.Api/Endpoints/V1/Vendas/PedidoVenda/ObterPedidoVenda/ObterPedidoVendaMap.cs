using Acme.Sistemas.Services.V1.PedidoVenda.Query.ObterPedidoVenda;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.PedidoVenda.ObterPedidoVenda;

public static class ObterPedidoVendaMap
{
    public static ObterPedidoVendaQuery ToQuery(this ObterPedidoVendaRequest request)
        => new(request.Id);

    public static ObterPedidoVendaResponse ToResponse(this ObterPedidoVendaQueryResult r)
        => new(r.Id, r.Numero,
            r.ClienteId, r.ClienteNome,
            r.VendedorId, r.OrcamentoId,
            r.DataEmissao, r.EstoqueId,
            r.ValorTotal, r.DescontoPercentual,
            r.Status, r.CondicaoPagamento, r.Observacao,
            r.Itens.Select(i => new ObterPedidoVendaResponseItem(
                i.Id, i.ProdutoId, i.ProdutoNome,
                i.Quantidade, i.QuantidadeFaturada, i.QuantidadePendente,
                i.PrecoUnitario, i.Total)).ToArray());
}
