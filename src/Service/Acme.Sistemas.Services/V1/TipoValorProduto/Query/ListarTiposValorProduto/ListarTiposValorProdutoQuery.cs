using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.TipoValorProduto.Query.ListarTiposValorProduto;

public sealed record ListarTiposValorProdutoQuery() : IRequest<ResponseDefault<ListarTiposValorProdutoQueryResult>>;

