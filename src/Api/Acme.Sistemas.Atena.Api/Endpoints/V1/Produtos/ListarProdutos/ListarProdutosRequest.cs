namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Produtos.ListarProdutos;

public sealed record ListarProdutosRequest(
    string? Termo = null,
    int Skip = 0,
    int Take = 50);
