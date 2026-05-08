using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.TipoProduto.Query.ListarTiposProduto;

public sealed record ListarTiposProdutoQuery() : IRequest<ResponseDefault<ListarTiposProdutoQueryResult>>;

