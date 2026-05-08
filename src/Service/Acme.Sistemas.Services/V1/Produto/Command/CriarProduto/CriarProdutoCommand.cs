using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Produto.Command.CriarProduto;

public sealed record CriarProdutoCommand(
    string Codigo,
    string Nome,
    string? Descricao,
    string? CodigoBarras,
    string UnidadeMedida,
    Guid? TipoProdutoId,
    Guid? FornecedorId,
    decimal? CustoMedio,
    decimal? EstoqueMinimo) : IRequest<ResponseDefault<CriarProdutoCommandResult>>;

