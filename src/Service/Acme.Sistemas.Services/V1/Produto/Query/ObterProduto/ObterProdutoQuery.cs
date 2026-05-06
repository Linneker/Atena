using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Produto.Query.ObterProduto;

public sealed record ObterProdutoQuery(Guid Id) : IRequest<ResponseDefault<ObterProdutoQueryResult>>;

public sealed record PrecoVigente(
    Guid Id, Guid TipoValorProdutoId, decimal Valor,
    DateTime VigenciaInicio, DateTime? VigenciaFim);

public sealed record ObterProdutoQueryResult(
    Guid Id, string Codigo, string Nome, string? Descricao,
    string? CodigoBarras, string UnidadeMedida,
    Guid? TipoProdutoId, Guid? FornecedorId,
    decimal? CustoMedio, decimal? EstoqueMinimo,
    StatusAtivo Status, IReadOnlyList<PrecoVigente> Precos);
