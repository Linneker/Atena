using Acme.Sistemas.Services.V1.Faturamento.Query.ObterFaturamento;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.Faturamento.ObterFaturamento;

public static class ObterFaturamentoMap
{
    public static ObterFaturamentoQuery ToQuery(this ObterFaturamentoRequest request) => new(request.Id);

    public static ObterFaturamentoResponse ToResponse(this ObterFaturamentoQueryResult r)
        => new(r.Id, r.Numero, r.PedidoVendaId,
            r.DataFaturamento, r.Tipo, r.ValorTotal,
            r.NFeId, r.ContaReceberId, r.Observacao,
            r.Itens.Select(i => new ObterFaturamentoResponseItem(
                i.Id, i.PedidoVendaItemId, i.ProdutoId, i.ProdutoNome,
                i.Quantidade, i.PrecoUnitario, i.Total)).ToArray());
}
