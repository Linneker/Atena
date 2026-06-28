using SrvRel = Acme.Sistemas.Services.V1.Estoque.Query.RelatorioMovimentacao;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Estoque.RelatorioMovimentacao;

public static class RelatorioMovimentacaoMap
{
    public static SrvRel.RelatorioMovimentacaoQuery ToQuery(this RelatorioMovimentacaoRequest request)
        => new(request.ProdutoId, request.Inicio, request.Fim, request.Skip, request.Take);

    public static RelatorioMovimentacaoResponse ToResponse(this SrvRel.RelatorioMovimentacaoResult result)
        => new(
            result.ProdutoId,
            result.Inicio,
            result.Fim,
            result.TotalEntradas,
            result.TotalSaidas,
            result.Saldo,
            result.Movimentos.Select(m => m.ToResponseLinha()).ToArray());

    private static RelatorioMovimentacaoResponseLinha ToResponseLinha(this SrvRel.MovimentoLinha linha)
        => new(
            linha.Data,
            linha.Tipo,
            linha.EstoqueId,
            linha.Quantidade,
            linha.CustoUnitario,
            linha.CmvUnitario,
            linha.Origem,
            linha.Motivo,
            linha.DocumentoReferencia);
}
