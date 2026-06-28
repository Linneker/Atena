namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Produtos.DefinirPrecoProduto;

public sealed record DefinirPrecoProdutoRequest(
    Guid TipoValorProdutoId,
    decimal Valor,
    DateTime? VigenciaInicio);
