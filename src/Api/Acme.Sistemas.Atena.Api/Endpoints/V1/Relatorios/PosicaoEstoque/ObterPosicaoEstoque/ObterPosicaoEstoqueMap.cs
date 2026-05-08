using Acme.Sistemas.Services.V1.Relatorios.PosicaoEstoque;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Relatorios.PosicaoEstoque.ObterPosicaoEstoque;

public static class ObterPosicaoEstoqueMap
{
    public static PosicaoEstoqueQuery ToQuery(this ObterPosicaoEstoqueRequest request)
        => new(request.EstoqueId);

    public static ObterPosicaoEstoqueResponse ToResponse(this PosicaoEstoqueQueryResult result)
        => new(result.EstoqueId, result.TotalProdutos, result.ValorTotalEstoque,
            result.Linhas.Select(l => new PosicaoEstoqueLinhaResponse(
                l.ProdutoId, l.CodigoProduto, l.NomeProduto,
                l.SaldoTotal, l.SaldoReservado, l.SaldoDisponivel,
                l.CustoMedio, l.ValorEstoque)).ToArray());
}
