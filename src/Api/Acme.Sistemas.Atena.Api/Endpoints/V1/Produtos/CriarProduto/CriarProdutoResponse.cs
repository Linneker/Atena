namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Produtos.CriarProduto;

public sealed record CriarProdutoResponse(
    Guid Id,
    string Codigo,
    string Nome);
