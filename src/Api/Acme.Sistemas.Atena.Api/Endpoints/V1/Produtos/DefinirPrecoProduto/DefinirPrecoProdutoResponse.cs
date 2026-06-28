namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Produtos.DefinirPrecoProduto;

public sealed record DefinirPrecoProdutoResponse(
    Guid PrecoId,
    decimal Valor,
    DateTime VigenciaInicio);
