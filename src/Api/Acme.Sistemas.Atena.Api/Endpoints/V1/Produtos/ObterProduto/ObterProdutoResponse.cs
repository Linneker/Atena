using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Produtos.ObterProduto;

public sealed record ObterProdutoResponsePreco(
    Guid Id,
    Guid TipoValorProdutoId,
    decimal Valor,
    DateTime VigenciaInicio,
    DateTime? VigenciaFim);

public sealed record ObterProdutoResponse(
    Guid Id,
    string Codigo,
    string Nome,
    string? Descricao,
    string? CodigoBarras,
    string UnidadeMedida,
    Guid? TipoProdutoId,
    Guid? FornecedorId,
    string? FornecedorNome,
    decimal? CustoMedio,
    decimal? EstoqueMinimo,
    StatusAtivo Status,
    IReadOnlyList<ObterProdutoResponsePreco> Precos);
