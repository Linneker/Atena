using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Produtos.AlterarProduto;

public sealed record AlterarProdutoRequest(
    string Nome,
    string? Descricao,
    string? CodigoBarras,
    string UnidadeMedida,
    Guid? TipoProdutoId,
    Guid? FornecedorId,
    decimal? CustoMedio,
    decimal? EstoqueMinimo,
    StatusAtivo Status);
